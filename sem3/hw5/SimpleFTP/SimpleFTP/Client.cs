using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    /// <summary>
    /// Class implementing FTP client.
    /// </summary>
    public class Client
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
                Console.WriteLine(exception.Message);
                IsConnected = false;
            }
        }

        /// <summary>
        /// Sends a request to list all files and directories at the given path on the server
        /// </summary>
        /// <param name="path">Path to the directory on the server to list all files and directories in.</param>
        /// <returns>String "size (name isDir)* " where "size" is a number of files and folders in the directory,
        /// "name" is a name of a file or a folder and "isDir" is bool value indicating whether it is directory."</returns>
        public async Task<string> List(string path)
        {
            var response = await MakeRequest(1, path);

            if (response == "-1")
            {
                throw new DirectoryNotFoundException();
            }
            else
            {
                return response;
            }
        }

        /// <summary>
        /// Sends a request to get a file from server. 
        /// </summary>
        /// <param name="path">Path to the file to get.</param>
        /// <returns>String "size content", where "size" is a size of the file and "content" is a file content.</returns>
        public async Task<string> Get(string path)
        {
            var response = await MakeRequest(2, path);

            if (response == "-1")
            {
                throw new FileNotFoundException();
            }
            else
            {
                return response;
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
    }
}
