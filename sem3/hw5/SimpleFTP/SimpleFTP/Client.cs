using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    public class Client
    {
        private static TcpClient client;

        private readonly string hostname;

        private readonly int port;

        private StreamWriter writer;

        private StreamReader reader;

        public bool IsConnected { get; private set; }

        public Client(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
        }

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

        public void Stop()
        {
            writer?.Close();
            reader?.Close();
            client?.Close();
        }
    }
}
