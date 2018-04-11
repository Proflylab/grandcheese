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
            var b = Encoding.Unicode.GetBytes(s);

            if (withLen)
            {
                WriteInt(b.Length);
            }
            packet.AddRange(b);
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
    }
}
