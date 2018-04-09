using System;
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

        public Packet(byte[] packet)
        {
            this.packet = new List<byte>();
            this.packet.AddRange(packet);
        }

        public Packet(int op)
        {
            opcode = op;
            packet = new List<byte>();
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

        public void WriteInt(int i)
        {
            packet.Add((byte)((i >> 24) & 0xFF));
            packet.Add((byte)((i >> 16) & 0xFF));
            packet.Add((byte)((i >> 8) & 0xFF));
            packet.Add((byte)(i & 0xFF));
        }

        public void WriteString(string s)
        {
            packet.AddRange(Encoding.ASCII.GetBytes(s));
        }

        public void WriteUnicodeString(string s)
        {
            // UTF-16 little encoding
            packet.AddRange(Encoding.Unicode.GetBytes(s));
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

        public string ReadString(int n)
        {
            byte[] ret = new byte[n];

            for (int x = 0; x < n; x++)
            {
                ret[x] = ReadByte();
            }

            return Encoding.ASCII.GetString(ret);
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
    }
}
