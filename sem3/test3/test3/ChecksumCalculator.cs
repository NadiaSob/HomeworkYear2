using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace test3
{
    /// <summary>
    /// Class that calculates checksum of the file system directory.
    /// </summary>
    public class ChecksumCalculator
    {
        private string input;

        public ChecksumCalculator(string path)
        {
            var fileArray = Directory.GetFiles(path);
            Array.Sort(fileArray);

            try
            {
                foreach (var file in fileArray)
                {
                    input += ReadFile(file);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private string GetStringHash(MD5 md5Hash)
        {
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Calculates checksum of the file system directory.
        /// </summary>
        /// <returns>Checksum of the file system directory.</returns>
        public string Calculate()
        {
            var md5Hash = MD5.Create();
            return GetStringHash(md5Hash);
        }

        private string ReadFile(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
