using System;
using System.Globalization;
using System.IO;
using System.Text;
/*
   Author:Sagara
*/
namespace Commons.Utils
{
    public static class BinaryExt
    {
        public static string ReadNtString(this BinaryReader br)
        {
            var sb = new StringBuilder();

            short ch;
            while (br.BaseStream.Position < br.BaseStream.Length && (ch = br.ReadInt16()) != 0)
                sb.Append((char)ch);

            return sb.ToString();
        }

        public static void Skip(this BinaryReader br, int count)
        {
            br.BaseStream.Position += count;
        }

        public static void Skip(this BinaryWriter br, int count)
        {
            br.BaseStream.Position += count;
        }

        public static void WriteNtString(this BinaryWriter wr, string val, Encoding enc = null)
        {
            if (enc == null)
                enc = Encoding.Unicode;

            wr.Write(enc.GetBytes(val));
            wr.Write((short)0);
        }

        public static string ReadString(this BinaryReader br, int len, Encoding enc = null)
        {
            if (enc == null)
                enc = Encoding.Unicode;

            return enc.GetString(br.ReadBytes(len));
        }

        public static string ReadHex(this BinaryReader br, int len)
        {
            return br.ReadBytes(len).ToHex();
        }


        public static void WriteD(this BinaryWriter bw, long val)
        {
            bw.Write((int)val);
        }

        public static void WriteD(this BinaryWriter bw, Enum val)
        {
            bw.Write(val.GetHashCode());
        }

        public static void WriteH(this BinaryWriter bw, long val)
        {
            bw.Write((short)val);
        }

        public static void WriteOffset(this BinaryWriter bw, long position, long val)
        {
            bw.BaseStream.Seek(position, SeekOrigin.Begin);
            bw.Write((short)val);
            bw.BaseStream.Seek(0, SeekOrigin.End);
        }

        public static void WriteC(this BinaryWriter bw, long val)
        {
            bw.Write((byte)val);
        }

        public static void WriteC(this BinaryWriter bw, bool val)
        {
            bw.Write((byte)val.GetHashCode());
        }

        public static void WriteDf(this BinaryWriter bw, double val)
        {
            bw.Write(val);
        }

        public static void WriteF(this BinaryWriter bw, double val)
        {
            bw.Write((float)val);
        }

        public static void WriteQ(this BinaryWriter bw, long val)
        {
            bw.Write(val);
        }

        public static void WriteS(this BinaryWriter bw, string text, Encoding enc = null)
        {
            bw.WriteNtString(text, enc);
        }

        public static void WriteB(this BinaryWriter bw, string hex)
        {
            bw.Write(hex.ToBytes());
        }

        public static void WriteB(this BinaryWriter bw, byte[] data)
        {
            bw.Write(data);
        }

        public static byte[] ToBytes(this string hexString)
        {
            byte[] numArray = new byte[hexString.Length / 2];
            for (int index = 0; index < numArray.Length; ++index)
            {
                string s = hexString.Substring(index * 2, 2);
                numArray[index] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return numArray;
        }

        public static byte[] XorKey(byte[] key1, byte[] key2)
        {
            byte[] result = new byte[Math.Min(key1.Length, key2.Length)];

            for (int i = 0; i < result.Length; i++)
                result[i] = (byte)(key1[i] ^ key2[i]);

            return result;
        }

        public static byte[] ShiftKey(byte[] src, int n, bool direction = true)
        {
            byte[] result = new byte[src.Length];

            for (int i = 0; i < src.Length; i++)
                if (direction)
                    result[(i + n) % src.Length] = src[i];
                else
                    result[i] = src[(i + n) % src.Length];

            return result;
        }

        public static string ToHex(this byte[] array)
        {
            StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
            for (int index = 0; index < array.Length; ++index)
                stringBuilder.Append(array[index].ToString("x2"));
            return stringBuilder.ToString();
        }

        public static string FormatHex(this byte[] data, int bytesPerBlock = 4, int blocksPerRow = 4)
        {
            StringBuilder builder = new StringBuilder(data.Length * 4);

            int len = data.Length;
            int bytesPerRow = bytesPerBlock * blocksPerRow;

            int lastRowLen = len % bytesPerRow;
            int rows = len / bytesPerRow;

            if (lastRowLen > 0)
                rows++;

            for (int i = 0; i < rows; i++)
            {
                int currentCount = bytesPerRow * i;
                builder.Append("[");
                builder.Append(currentCount);
                builder.Append("]\t");

                int bytesInThisRow = lastRowLen > 0 && rows - 1 == i ? lastRowLen : bytesPerRow;
                for (int k = 0; k < bytesInThisRow; k++)
                {
                    byte res = data[currentCount + k];
                    builder.Append(res.ToString("X2"));

                    if ((k + 1) % bytesPerBlock == 0)
                        builder.Append("  ");
                }

                var diff = bytesPerRow - bytesInThisRow;
                if (diff > 0)
                {

                    var cnt = diff + diff / bytesPerBlock;
                    if (diff % bytesPerBlock > 0)
                        cnt++;

                    for (int k = 0; k < cnt; k++)
                        builder.Append("  ");
                }

                for (int k = 0; k < bytesInThisRow; k++)
                {

                    char res = (char)data[currentCount + k];
                    if (res > 0x1f && res < 0x80)
                        builder.Append(res);
                    else
                        builder.Append(".");
                }

                builder.Append("\n");
            }

            return builder.ToString();
        }
    }
}
