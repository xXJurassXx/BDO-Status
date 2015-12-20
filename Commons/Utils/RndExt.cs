using System;

namespace Commons.Utils
{
    public static class RndExt
    {
        private static readonly Random Rng = new Random();
        private static readonly string _chars = "0123456789";

        public static string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[Rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
    }
}
