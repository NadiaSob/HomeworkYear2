using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace test1
{
    /// <summary>
    /// Client class.
    /// </summary>
    public class Client
    {
        private static TcpClient client;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="hostname">The DNS name of the remote host to which you intend to connect.</param>
        /// <param name="port">The port number of the remote host to which you intend to connect.</param>
        public Client(string hostname, int port)
        {
            client = new TcpClient(hostname, port);
            Process();
        }

        private static void Process()
        {
            try
            {
                while (true)
                {
                    var stream = client.GetStream();
                    Writer(stream);
                    Reader(stream);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Disconnect();
            }
        }

        private static void Writer(NetworkStream stream)
        {
            Task.Run(async () =>
            {
                var writer = new StreamWriter(stream) { AutoFlush = true };
                while (true)
                {
                    Console.WriteLine("Sending: ");
                    var data = Console.ReadLine();
                    await writer.WriteAsync(data + "\n");
                    if (data == "exit")
                    {
                        Disconnect();
                    }
                }
            });
        }

        private static void Reader(NetworkStream stream)
        {
            Task.Run(async () =>
            {
                var reader = new StreamReader(stream);
                var data = await reader.ReadLineAsync();
                Console.WriteLine($"Received: {data}\n");
            });
        }

        private static void Disconnect()
        {
            client.Close();
            Environment.Exit(0);
        }
    }
}
