using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace test1
{
    /// <summary>
    /// Server class.
    /// </summary>
    public class Server
    {
        private static TcpListener listener;

        private static TcpClient client;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port">The port on which to listen for incoming connection attempts.</param>
        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            Listen();
        }

        private static async void Listen()
        {
            try
            {
                listener.Start();
                Console.WriteLine("Waiting for connection...");

                while (true)
                {
                    client = await listener.AcceptTcpClientAsync();

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
            listener.Stop();
            client.Close();
            Environment.Exit(0);
        }
    }
}
