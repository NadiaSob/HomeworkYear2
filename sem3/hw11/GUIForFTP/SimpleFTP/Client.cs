using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimpleFTP
{
    /// <summary>
    /// Class implementing FTP client.
    /// </summary>
    public class Client : IDisposable
    {
        private static TcpClient client;

        private readonly string hostname;

        private readonly int port;

        private StreamWriter writer;

        private StreamReader reader;

        /// <summary>
        /// Indicates whether the client is connected to the server.
        /// </summary>
        public bool IsConnected { get; private set; }

        public Client(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

        /// <summary>
        /// Connects client to the server.
        /// </summary>
        public void Connect()
        {
            try
            {
                client = new TcpClient(hostname, port);
                var stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);
                IsConnected = true;
            }
            catch (SocketException exception)
            {
                IsConnected = false;
                throw exception;
            }
        }

        /// <summary>
        /// Sends a request to list all files and directories at the given path on the server
        /// </summary>
        /// <param name="path">Path to the directory on the server to list all files and directories in.</param>
        /// <returns>String "size (name isDir)* " where "size" is a number of files and folders in the directory,
        /// "name" is a name of a file or a folder and "isDir" is bool value indicating whether it is directory."</returns>
        public async Task<List<(string, bool)>> List(string path)
        {
            var response = await MakeRequest(1, path);

            if (response == "-1")
            {
                throw new DirectoryNotFoundException();
            }
            else
            {
                return HandleListResponse(response);
            }
        }

        private List<(string, bool)> HandleListResponse(string response)
        {
            var splitResponse = response.Split(' ');

            var length = int.Parse(splitResponse[0]);

            var result = new List<(string, bool)>();
            for (var i = 1; i < length * 2; i += 2)
            {
                var name = splitResponse[i];
                var isDirectory = bool.Parse(splitResponse[i + 1]);
                result.Add((name, isDirectory));
            }

            return result;
        }

        /// <summary>
        /// Get a file from server to the destination directory. 
        /// </summary>
        /// <param name="filePath">Path to get file from.</param>
        /// <param name="destination">Path to get file into.</param>
        public async Task Get(string filePath, string destination)
        {
            var response = await MakeRequest(2, filePath);
            var splitResponse = response.Split(' ');
            var content = response.Replace($"{splitResponse[0]} ", "");

            if (response == "-1")
            {
                throw new FileNotFoundException();
            }
            else
            {
                var fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
                using (var fileWriter = new StreamWriter(new FileStream(destination + "\\" + fileName, FileMode.Create)))
                {
                    await fileWriter.WriteAsync(content);
                }
            }
        }

        private async Task<string> MakeRequest(int command, string path)
        {
            if (IsConnected)
            {
                await writer.WriteLineAsync(command + " " + path);
                return await reader.ReadLineAsync();
            }
            else
            {
                throw new InvalidOperationException("Client is not connected to the server");
            }
        }

        /// <summary>
        /// Stops the client.
        /// </summary>
        public void Stop()
        {
            writer?.Close();
            reader?.Close();
            client?.Close();
        }

        /// <summary>
        /// Disposes writer, reader and client.
        /// </summary>
        public void Dispose()
        {
            writer?.Dispose();
            reader?.Dispose();
            client?.Dispose();
        }
    }
}
