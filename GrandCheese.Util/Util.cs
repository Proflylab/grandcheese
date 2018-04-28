using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    public class Util
    {
        public static byte[] IntToByteArrayLittle(int data)
        {
            byte[] b = new byte[4];
            b[0] = (byte)data;
            b[1] = (byte)(((uint)data >> 8) & 0xFF);
            b[2] = (byte)(((uint)data >> 16) & 0xFF);
            b[3] = (byte)(((uint)data >> 24) & 0xFF);
            return b;
        }

        public static byte[] ShortToByteArrayBig(short s)
        {
            byte[] bytes = BitConverter.GetBytes(s);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        public static byte[] IntToByteArrayBig(int i)
        {
            byte[] bytes = BitConverter.GetBytes(i);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        public static byte[] ShortToByteArrayLittle(short data)
        {
            // untested
            byte[] b = new byte[2];
            b[0] = (byte)data;
            b[1] = (byte)(((uint)data >> 8) & 0xFF);

            return b;
        }
        
        public static int ByteArrayToShortBig(byte[] bytes)
        {
            return ((((int)bytes[0] & 0xff) << 8) |
                    ((int)bytes[1] & 0xff));
        }

        public static int BytesToShortBig(byte b0, byte b1)
        {
            return ((((int)b0 & 0xff) << 8) |
                    ((int)b1 & 0xff));
        }

        public static int ByteArrayToShortBig(byte[] bytes, int offset)
        {
            return ((((int)bytes[offset + 0] & 0xff) << 8) |
                    ((int)bytes[offset + 1] & 0xff));
        }

        public static int ByteArrayToIntLittle(byte[] bytes)
        {
            return ((((int)bytes[0] & 0xff)) |
                    (((int)bytes[1] & 0xff) << 8) |
                    (((int)bytes[2] & 0xff) << 16) |
                    (((int)bytes[3] & 0xff) << 24));
        }

        public static int ByteArrayToIntLittle(byte[] bytes, int offset)
        {
            return ((((int)bytes[offset + 0] & 0xff)) |
                    (((int)bytes[offset + 1] & 0xff) << 8) |
                    (((int)bytes[offset + 2] & 0xff) << 16) |
                    (((int)bytes[offset + 3] & 0xff) << 24));
        }

        public static string ConvertBytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        public static byte[] HexStringToByteArray(string s)
        {
            return Enumerable.Range(0, s.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                        .ToArray();
        }
    }
}
