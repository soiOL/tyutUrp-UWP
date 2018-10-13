using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp
{
    class Secret
    {
        private static readonly String key = "soiol";

        //加密
        public static String encryption(String content)
        {
            byte[] contentBytes = Encoding.Default.GetBytes(content); ;
            byte[] keyBytes = Encoding.Default.GetBytes(key);

            byte dkey = 0;
            foreach (byte b in keyBytes)
            {
                dkey ^= b;
            }

            byte salt = 0;  //随机盐值
            byte[] result = new byte[contentBytes.Length];
            for (int i = 0; i < contentBytes.Length; i++)
            {
                salt = (byte)(contentBytes[i] ^ dkey ^ salt);
                result[i] = salt;
            }
            return Encoding.UTF8.GetString(result);
        }

        //解密
        public static String decipher(String content)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content); ;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte dkey = 0;
            foreach (byte b in keyBytes)
            {
                dkey ^= b;
            }

            byte salt = 0;  //随机盐值
            byte[] result = new byte[contentBytes.Length];
            for (int i = contentBytes.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    salt = 0;
                }
                else
                {
                    salt = contentBytes[i - 1];
                }
                result[i] = (byte)(contentBytes[i] ^ dkey ^ salt);
            }
            return System.Text.Encoding.UTF8.GetString(result);
        }
    }
}
