using GrandCheese.Util.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    public class ServerApp
    {
        public Logger logger = Log.Get();

        public Socket serverSocket;
        public readonly byte[] buffer = new byte[2048];

        public bool isGame = false;
        public Dictionary<string, Client> _clients = new Dictionary<string, Client>();
        public Dictionary<short, MethodBase> serverPackets = new Dictionary<short, MethodBase>();

        public object Reader = null;

        public Action<ServerApp, Client, Packet, short> CustomInvoke = null;
        public Action<string> OnDisconnect = null;
        public Action<Client> CreateUserClient = null;

        public void PopulatePackets()
        {
            var methods = Assembly.GetEntryAssembly().GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(OpcodeAttribute), false).Length > 0)
                      .ToArray();

            foreach (var method in methods)
            {
                foreach (var attr in method.GetCustomAttributes(true))
                {
                    if (attr is OpcodeAttribute)
                    {
                        serverPackets.Add(((OpcodeAttribute)attr).id, method);
                        break;
                    }
                }
            }

            Log.Get().Info("Handled opcodes: {0}", string.Join(", ", serverPackets.Keys.Select(x => "0x" + x.ToString("X2"))));
        }

        public void PopulateInterfaces()
        {
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(t => t.GetTypes())
                        .Where(i => typeof(ISerializable).IsAssignableFrom(i))
                        .ToArray();

            foreach (var cls in classes)
            {
                // ignore the base instance
                if(cls.Name != "ISerializable")
                {
                    ProcessSettings.interfaces.Add(cls.Name, cls);
                }
            }

            Log.Get().Info("Handled interfaces: {0}", string.Join(", ", ProcessSettings.interfaces.Keys));
        }

        public void StartServer(int port, string type = "center")
        {
            logger.Info("Populating packets and interfaces...");
            PopulatePackets();
            PopulateInterfaces();

            logger.Info("Starting server...");
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(5);
                serverSocket.BeginAccept(AcceptCallback, null);
                logger.Info("Server has been started on port {0}.", port);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to start the server.");
                Console.ReadKey();
                Environment.Exit(1); // Exit with error code 1 because error
            }

            if(type == "game")
            {
                ProcessSettings.isGame = true;
            }
        }

        public void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            logger.Info("A client has been accepted from port {0}.", socket.RemoteEndPoint.ToString());

            var client = new Client()
            {
                Id = socket.RemoteEndPoint.ToString(),
                Sock = socket,
                Crypto = null
            };

            client.Crypto = new Crypto(client);

            _clients.Add(socket.RemoteEndPoint.ToString(), client);

            client.Crypto.SendFirstPacket(); // send first and second packets

            socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, socket);

            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;

            int received;
            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                logger.Warn("Client {0} disconnected.", current.RemoteEndPoint.ToString());
                OnDisconnect?.Invoke(current.RemoteEndPoint.ToString());
                _clients.Remove(current.RemoteEndPoint.ToString());
                current.Close();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);

            if (received != 0)
            {
                var packets = new List<byte[]>();

                short packetSize = (short)((recBuf[1] << 8) + recBuf[0]);

                if (recBuf.Length > packetSize)
                {
                    var currentIteration = 0;
                    var readBytes = 0;

                    try
                    {
                        // make sure this isn't going to be an infinite loop
                        for (int i = 0; i < 500; i++)
                        {
                            if (packetSize * currentIteration >= recBuf.Length)
                            {
                                break;
                            }

                            packetSize = (short)((recBuf[packetSize * currentIteration + 1] << 8) + recBuf[packetSize * currentIteration]);
                            var nextPacket = recBuf.Skip(readBytes).Take(packetSize).ToArray();
                            currentIteration++;
                            readBytes += packetSize;

                            packets.Add(nextPacket);
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex, "Unable to process packet. Perhaps a malformed packet was sent?");
                    }
                }
                else
                {
                    packets.Add(recBuf.Take(packetSize).ToArray());
                }

                foreach (byte[] b in packets)
                {
                    ProcessPacket(b, current);
                }

                packets = null;
            }
            else
            {
                return;
            }

            current.BeginReceive(buffer, 0, 2048, SocketFlags.None, ReceiveCallback, current);
            return;
        }

        public void ProcessPacket(byte[] b, Socket current)
        {
            try
            {
                var c = _clients[current.RemoteEndPoint.ToString()];

                var p = new Packet(b);

                c.Crypto.DecryptPacket(p);

                short opcode = p.ReadShort();
                int length = p.ReadInt();
                int isCompressed = p.ReadByte();
                if (isCompressed == 1) p.ReadInt();

                if (serverPackets.ContainsKey(opcode))
                {
                    Log.Get().Info("[Receive] {0} : {1} ({2})", c.Id, ProcessSettings.isGame ? ((GameOpcodes)opcode).ToString() : ((LoginOpcodes)opcode).ToString(), opcode);
                    if (CustomInvoke != null)
                    {
                        //serverPackets[opcode].Invoke(c.User, new object[] { c, p });
                        CustomInvoke(this, c, p, opcode);
                    }
                    else
                    {
                        serverPackets[opcode].Invoke(null, new object[] { c, p });
                    }
                }
                else
                {
                    Log.Get().Warn("Unknown packet received. Opcode: {0} Length: {1}", ProcessSettings.isGame ? ((GameOpcodes)opcode).ToString() : ((LoginOpcodes)opcode).ToString(), length);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to handle packet. Perhaps a malformed packet was sent?");
                logger.Trace(Util.ConvertBytesToHexString(b));
            }
        }
    }
}
