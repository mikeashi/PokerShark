using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PokerShark.Helpers
{
    internal static class ExecutingHash
    {
        public static string GetExecutingFileHash()
        {
            return MD5(GetSelfBytes());
        }

        private static string MD5(byte[] input)
        {
            return MD5(ASCIIEncoding.ASCII.GetString(input));
        }

        private static string MD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] originalBytes = ASCIIEncoding.Default.GetBytes(input);
                byte[] encodedBytes = md5.ComputeHash(originalBytes);
                return BitConverter.ToString(encodedBytes).Replace("-", "");
            }
        }

        private static byte[] GetSelfBytes()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            FileStream running = File.OpenRead(path);

            byte[] exeBytes = new byte[running.Length];
            running.Read(exeBytes, 0, exeBytes.Length);
            running.Close();
            return exeBytes;
        }
    }
}
