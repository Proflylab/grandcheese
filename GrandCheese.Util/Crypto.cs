using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using zlib;

namespace GrandCheese.Util
{
    public class Crypto
    {
        byte[] GC_DES_KEY = { (byte)0xC7, (byte)0xD8, (byte)0xC4, (byte)0xBF, (byte)0xB5, (byte)0xE9, (byte)0xC0, (byte)0xFD };
        byte[] GC_HMAC_KEY = { (byte)0xC0, (byte)0xD3, (byte)0xBD, (byte)0xC3, (byte)0xB7, (byte)0xCE, (byte)0xB8, (byte)0xB8 };
        byte GC_HMAC_SIZE = 10;

        readonly Client client;
        readonly RNGCryptoServiceProvider rngCsp;
        DES _des;
        HMACMD5 _hmac;

        public Crypto(Client client)
        {
            this.client = client;

            _des = DES.Create();
            _des.Key = GC_DES_KEY;
            _des.Mode = CipherMode.CBC;
            _des.Padding = PaddingMode.None;
            
            rngCsp = new RNGCryptoServiceProvider();
            _hmac = new HMACMD5(GC_HMAC_KEY);
        }

        public void SendFirstPacket()
        {
            byte[] newDesKey = new byte[8];
            byte[] newHmacKey = new byte[8];
            byte[] newPrefix = GeneratePrefix();

            // Generate new
            rngCsp.GetBytes(newDesKey);
            rngCsp.GetBytes(newHmacKey);

            Packet p = new Packet(0x01);
            p.Write(newPrefix); // prefix
            p.WriteInt(8);
            p.Write(newHmacKey);
            p.WriteInt(8);
            p.Write(newDesKey);
            p.WriteInt(1);
            p.WriteSkip(8);
            SendPacket(p);

            _des.Key = newDesKey;
            _hmac = new HMACMD5(newHmacKey);
            client.Prefix = newPrefix;

            Log.Get().Info("DES key for {0}: {1}", client.Id, Util.ConvertBytesToHexString(_des.Key));
            Log.Get().Info("HMAC key for {0}: {1}", client.Id, Util.ConvertBytesToHexString(newHmacKey));

            // Second packet

            Packet p2 = new Packet((short)LoginOpcodes.ENU_WAIT_TIME_NOT);
            p2.WriteInt(10000);
            SendPacket(p2);
        }

        public void SendPacket(Packet p, bool compress = false)
        {
            p.ApplyWrite();

            byte[] packet = null;

            packet = AssemblePacket(p, client.Prefix, client.Number++, compress);

            client.Sock.Send(packet);
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[(int)input.Length + 10240];
            int len;
            while ((len = input.Read(buffer, 0, (int)input.Length + 10240)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        private static byte[] CompressData(byte[] inData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream, zlibConst.Z_BEST_SPEED))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                return outMemoryStream.ToArray();
            }
        }

        public byte[] AssemblePacket(Packet p, byte[] prefix, int packetno, bool compress)
        {
            Log.Get().Info("[Send] {0} : {1} ({2})", client.Id, ProcessSettings.isGame ? ((GameOpcodes)p.opcode).ToString() : ((LoginOpcodes)p.opcode).ToString(), p.opcode);

            Log.Get().Trace("[Send Trace] Packet: {0}", Util.ConvertBytesToHexString(p.packet.ToArray()));

            var iv = GenerateIV();
            
            _des.IV = iv;
            
            List<byte> newpkt = new List<byte>();

            int paddinglen = 8 - ((7 + p.packet.Count + 1) % 8);
            
            if(paddinglen < 3)
            {
                paddinglen += 8;
            }

            List<byte> packetwh = new List<byte>();

            if(compress)
            {
                byte[] temp = CompressData(p.packet.ToArray());

                packetwh.AddRange(Util.ShortToByteArrayBig((short)p.opcode)); // ID
                packetwh.AddRange(Util.IntToByteArrayBig(temp.Length + 4)); // Size - CHECK

                packetwh.Add(0x01);

                packetwh.AddRange(Util.IntToByteArrayLittle(p.packet.Count)); // size WHEN DECOMPRESSED??? WTF?
                packetwh.AddRange(temp);

                packetwh.AddRange(new byte[4]);

                paddinglen = 8 - ((packetwh.Count + 1) % 8);

                if (paddinglen < 3)
                {
                    paddinglen += 8;
                }
            }
            else
            {
                packetwh.AddRange(Util.ShortToByteArrayBig((short)p.opcode)); // ID
                packetwh.AddRange(Util.IntToByteArrayBig(p.packet.Count)); // Size - CHECK
                packetwh.Add(0x00);
                packetwh.AddRange(p.packet);
            }

            // padding!!

            byte[] padding = new byte[paddinglen + 1];

            for (int i = 1; i <= paddinglen; i++)
            {
                padding[i - 1] = (byte)(i - 1);
            }
            padding[paddinglen] = (byte)(paddinglen - 1);

            packetwh.AddRange(padding);

            byte[] realdata = packetwh.ToArray();

            if (compress)
            {
                Log.Get().Info("Packet COMPRESSED: {0}", Util.ConvertBytesToHexString(realdata));
            }
            else
            {
                Log.Get().Info("Packet NOT COMPRESSED: {0}", Util.ConvertBytesToHexString(realdata));
            }

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, _des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(realdata, 0, realdata.Length);
                }

                realdata = ms.ToArray().Take(realdata.Length).ToArray(); // replace! this is our real real data now
            }
            
            ushort length = (ushort)(realdata.Length + 26);

            newpkt.AddRange(BitConverter.GetBytes(length));

            // PREFIX
            newpkt.AddRange(prefix);

            // SEQUENCE
            newpkt.AddRange(Util.IntToByteArrayLittle(packetno));

            newpkt.AddRange(iv);

            newpkt.AddRange(realdata);

            byte[] data = newpkt.ToArray();

            byte[] datanolen = newpkt.Skip(2).ToArray();

            newpkt.AddRange(GetHmacMd5Data(datanolen));

            return newpkt.ToArray();
        }

        public void DecryptPacket(Packet p)
        {
            byte[] iv = p.packet.Skip(8).Take(8).ToArray();

            byte[] packetdata = p.packet.Skip(16).Take(p.packet.Count - 16 - GC_HMAC_SIZE).ToArray();

            Log.Get().Trace("[Receive] Packet: {0}", Util.ConvertBytesToHexString(p.packet.ToArray()));

            _des.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, _des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(packetdata, 0, packetdata.Length);
                }

                packetdata = ms.ToArray();
            }

            byte[] op = new byte[2];
            p.opcode = Util.BytesToShortBig(p.packet[0], p.packet[1]);

            var packetdatalist = new List<byte>();
            packetdatalist.AddRange(packetdata);

            p.packet = packetdatalist;

            // compressed?
            if (p.packet[6] == 0x01)
            {
                // Grab zlib data
                byte[] compressedData = p.packet.Skip(11).Take(p.packet.Count - 11).ToArray();

                // Decompress data
                packetdata = ZlibStream.UncompressBuffer(compressedData);

                // Check integrity of data
                int originalSize = Util.ByteArrayToIntLittle(p.packet.ToArray(), 7);

                if (packetdata.Length != originalSize)
                {
                    throw new Exception("Data integrity check failed: Size is different");
                }

                // Assemble
                var newpacket = new List<byte>();

                newpacket.AddRange(p.packet.Take(11));
                newpacket.AddRange(packetdata);

                p.packet = newpacket;

                newpacket = null;
            }

            iv = null;
            packetdata = null;
            op = null;
        }

        byte[] GenerateIV()
        {
            byte[] iv = new byte[8];

            var ivByte = (byte)new Random().Next(0x00, 0xFF);
            
            for(int i = 0; i < 8; i++)
            {
                iv[i] = ivByte;
            }

            return iv;
        }

        byte[] GetHmacMd5Data(byte[] data)
        {
            // linq is cool
            return _hmac.ComputeHash(data).Take(10).ToArray();
        }

        byte[] GeneratePrefix()
        {
            byte[] prefix = new byte[2];
            
            byte ivByte = (byte)new Random().Next(0x00, 0xFF);

            for (int i = 0; i < 2; i++)
            {
                prefix[i] = ivByte;
            }

            return prefix;
        }
    }
}
