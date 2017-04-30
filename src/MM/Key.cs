using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM
{
    class Key
    {
        private static byte[] key = null;
        private static byte[] iv = null;
        public static byte[] getKey()
        {
            return key;
        }
        public static byte[] getIv()
        {
            return iv;
        }
        public static void setkey(string k)
        {
            key = trans( Encoding.ASCII.GetBytes(k), 32);
            iv = trans(Encoding.ASCII.GetBytes(k), 16);
        }
        private static byte[] trans(byte[] bs, int to)
        {
            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (byte)((bs[i] - 33) / 93.0 * 256);
            }

            if (bs.Length == to)
            {
                return bs;
            }
            else if (bs.Length > to)
            {
                byte[] ts = new byte[to];
                for (int i = 0; i < ts.Length; i++)
                {
                    ts[i] = bs[i];
                }
                for (int i = ts.Length; i < bs.Length; i++)
                {
                    ts[i % ts.Length] = (byte)(bs[i] * ts[i % ts.Length]);
                }
                return ts;
            }
            else
            {
                byte[] ts = new byte[to];
                for (int i = 0; i < bs.Length; i++)
                {
                    ts[i] = bs[i];
                }
                for (int i = bs.Length; i < ts.Length; i++)
                {
                    ts[i] = (byte)(bs[i % bs.Length] * bs[i % (bs.Length - 1)]);
                }
                return ts;
            }
        }
    }
}
