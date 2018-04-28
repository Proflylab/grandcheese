using GrandCheese.Util.Exceptions;
using GrandCheese.Util.Interfaces;
using GrandCheese.Util.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    public class Packet
    {
        public List<byte> packet;
        int pos = 0;

        public int opcode = -1;
        public bool useVoodoo = false;
        public object kUser = null;

        public Packet(byte[] packet, object kUser = null)
        {
            this.packet = new List<byte>();
            this.packet.AddRange(packet);
            this.kUser = kUser;
        }

        public Packet(int op, object kUser = null)
        {
            opcode = op;
            packet = new List<byte>();
            this.kUser = kUser;
        }

        public Packet(GameOpcodes op, object kUser = null)
        {
            opcode = (int)op;
            packet = new List<byte>();
            this.kUser = kUser;
        }

        public Packet(LoginOpcodes op, object kUser = null)
        {
            opcode = (int)op;
            packet = new List<byte>();
            this.kUser = kUser;
        }

        // Write

        public void Write(byte b)
        {
            packet.Add(b);
        }

        public void Write(byte[] b)
        {
            packet.AddRange(b);
        }

        public void WriteSkip(int b)
        {
            for(int i = 0; i < b; i++)
            {
                packet.Add(0x00);
            }
        }

        public void WriteShort(short i)
        {
            packet.Add((byte)((i >> 8) & 0xFF));
            packet.Add((byte)(i & 0xFF));
        }

        public void WriteInt(int i)
        {
            packet.Add((byte)((i >> 24) & 0xFF));
            packet.Add((byte)((i >> 16) & 0xFF));
            packet.Add((byte)((i >> 8) & 0xFF));
            packet.Add((byte)(i & 0xFF));
        }

        public void WriteIntLittle(int i)
        {
            packet.Add((byte)(i & 0xFF));
            packet.Add((byte)((i >> 8) & 0xFF));
            packet.Add((byte)((i >> 16) & 0xFF));
            packet.Add((byte)((i >> 24) & 0xFF));
        }

        public void WriteLong(long i)
        {
            packet.Add((byte)((i >> 56) & 0xFF));
            packet.Add((byte)((i >> 48) & 0xFF));
            packet.Add((byte)((i >> 40) & 0xFF));
            packet.Add((byte)((i >> 32) & 0xFF));

            packet.Add((byte)((i >> 24) & 0xFF));
            packet.Add((byte)((i >> 16) & 0xFF));
            packet.Add((byte)((i >> 8) & 0xFF));
            packet.Add((byte)(i & 0xFF));
        }

        public void WriteString(string s, bool withLen = false)
        {
            if(withLen)
            {
                WriteInt(s.Length);
            }

            packet.AddRange(Encoding.ASCII.GetBytes(s));
        }

        public void WriteUnicodeString(string s, bool withLen = false)
        {
            // UTF-16 little encoding
            var bytes = Encoding.Unicode.GetBytes(s);

            if (withLen)
            {
                WriteInt(bytes.Length);
            }

            packet.AddRange(bytes);
        }
        
        public void WriteHexString(string s)
        {
            // remove spaces first
            s = s.Replace(" ", "");

            packet.AddRange(Enumerable.Range(0, s.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                        .ToArray());
        }

        public void ApplyWrite()
        {
            // this exists because if there isn't enough data, .NET will error
            // thanks, DES
            if (packet.Count < 32)
            {
                WriteSkip(32);
                useVoodoo = true;
            }

            WriteHexString("00 00 00");
        }

        public void WriteBool(bool b)
        {
            packet.Add(b ? (byte)1 : (byte)0);
        }

        // Read

        public void Seek(long offset)
        {
            pos = (int)offset;
        }

        public int Read()
        {
            return packet[pos++] & 0xFF;
        }

        public byte ReadByte()
        {
            return (byte)Read();
        }

        public short ReadShort()
        {
            return (short)((Read() << 8) + Read());
        }

        public int ReadInt()
        {
            return (Read() << 24) + (Read() << 16) + (Read() << 8) + Read();
        }

        public long ReadLong()
        {
            return (Read() << 56) + (Read() << 48) + (Read() << 40) + (Read() << 32) +
                    (Read() << 24) + (Read() << 16) + (Read() << 8) + Read();
        }

        public string ReadString(int n)
        {
            byte[] ret = new byte[n];

            for (int x = 0; x < n; x++)
            {
                ret[x] = ReadByte();
            }

            return Encoding.ASCII.GetString(ret);
        }

        public string ReadString()
        {
            return ReadString(ReadInt());
        }

        public string ReadUnicodeString(int n)
        {
            byte[] ret = new byte[n];

            for (int x = 0; x < n; x++)
            {
                ret[x] = ReadByte();
            }

            return Encoding.Unicode.GetString(ret);
        }

        public string ReadUnicodeString()
        {
            return ReadUnicodeString(ReadInt());
        }

        public bool ReadBool()
        {
            return Read() == 1;
        }
        
        public void Put(params object[] args)
        {
            int listItemIndex = 0;

            foreach(var arg in args)
            {
                if(arg == null)
                {
                    WriteInt(0);
                    continue;
                }

                if (arg.GetType() == typeof(byte) || arg.GetType() == typeof(char))
                {
                    // char == byte
                    // /shrug

                    Write((byte)arg);
                }
                else if (arg.GetType() == typeof(int) || arg.GetType() == typeof(uint) || arg.GetType() == typeof(float))
                {
                    // if it's an unsigned int, pretend it's a normal int
                    // the client should handle it

                    // todo: does float work?

                    WriteInt((int)arg);
                }
                else if(arg.GetType() == typeof(string))
                {
                    WriteString((string)arg, true);
                }
                else if (arg.GetType() == typeof(long) || arg.GetType() == typeof(UInt64) || arg.GetType() == typeof(double))
                {
                    // todo: does double work here?

                    WriteLong((long)arg);
                }
                else if (arg.GetType() == typeof(short) || arg.GetType() == typeof(ushort))
                {
                    WriteShort((short)arg);
                }
                else if (arg.GetType() == typeof(bool))
                {
                    Write((byte)((bool)arg ? 0x01 : 0x060));
                }
                else if(arg.GetType() == typeof(GCWideString))
                {
                    WriteUnicodeString((GCWideString)arg.ToString(), true);
                }
                else if(arg.GetType() == typeof(GCPair))
                {
                    // TODO: Look into tuples or something idk

                    var pair = (GCPair)arg;
                    Put(pair.first, pair.second);
                }
                else if(arg is IList && arg.GetType().IsGenericType)
                {
                    // essentially, std::vector

                    listItemIndex = 0; 

                    var list = (IList)arg;

                    Put(
                        list.Count
                    );

                    foreach(var obj in list)
                    {
                        Put(obj);
                        listItemIndex++;
                    }
                }
                else
                {
                    // Attempt to fall back to an interface
                    // Ignore any errors, we'll let the exception deal with that.
                    try
                    {
                        var name = arg.GetType().Name;

                        Log.Get().Trace("Looking for interface: {0}", name);

                        if (ProcessSettings.interfaces.ContainsKey(name))
                        {
                            // Attempt to cast it to ISerializable.
                            // Since all of these classes have void Serialize(Packet),
                            // we can just call it with this as an argument

                            ((ISerializable)arg).Serialize(this, listItemIndex, kUser);

                            Log.Get().Trace("Serialized {0}.", name);
                        }
                        else
                        {
                            throw new UnhandledSerializerTypeException("Unhandled serializer type " + arg.GetType().Name);
                        }
                    }
                    catch
                    {
                        // Assume 
                        throw new UnhandledSerializerTypeException("Unhandled serializer type " + arg.GetType().Name);
                    }
                }
            }
        }
    }
}
