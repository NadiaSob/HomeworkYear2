using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test3
{
    /// <summary>
    /// Class that calculates checksum of the file system directory and guarantee correct work in multithreaded mode.
    /// </summary>
    public class MultithreadedChecksumCalculator
    {
        private string input;

        private readonly object locker = new object();

        private ManualResetEvent[] doneEvents;

        private int index = 0;

        public MultithreadedChecksumCalculator(string path)
        {
            var fileArray = Directory.GetFiles(path);
            Array.Sort(fileArray);

            try
            {
                doneEvents = new ManualResetEvent[fileArray.Length];
                for (var i = 0; i < fileArray.Length; ++i)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                    ThreadPool.QueueUserWorkItem(ReadFile, fileArray[i]);
                    Interlocked.Increment(ref index);
                }
                WaitHandle.WaitAll(doneEvents);
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
            lock (locker)
            {
                var md5Hash = MD5.Create();
                return GetStringHash(md5Hash);
            }
        }

        private void ReadFile(object filePath)
        {
            using (StreamReader streamReader = new StreamReader((string)filePath))
            {
                var localIndex = index;
                input += streamReader.ReadToEnd();
                doneEvents[index].Set();
            }
        }
    }
}
