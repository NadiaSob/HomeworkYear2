using SimpleFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUIForFTP
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string hostname;
        private int port;
        private string serverPath;
        private Client client;
        private Server server;
        private bool isConnected = false;

        public ObservableCollection<string> ServerFolderList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> DownloadsFolderList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> InProcessList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> DownloadedList { get; private set; } 
            = new ObservableCollection<string>();



        public string ServerPath
        {
            get
                => serverPath;
            set
            {
                serverPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Server hostname.
        /// </summary>
        public string Hostname
        {
            get
                => hostname;
            set
            {
                isConnected = false;
                hostname = value;
            }
        }

        /// <summary>
        /// Server port.
        /// </summary>
        public string Port
        {
            get
            {
                if (port != -1)
                {
                    return port.ToString();
                }
                return "";
            }
            set
            {
                isConnected = false;
                if(!int.TryParse(value, out port))
                {
                    port = -1;
                }
            }
        }

        public ApplicationViewModel(string hostname, int port)
        {
            this.hostname = hostname;
            this.port = port;
            serverPath = "";
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void HandleException(Exception exception)
        {
            MessageBox.Show("An exception just occurred: " + exception.Message,
                "Exception", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }

        public ICommand Connect
        {
            get
            {
                return new Command(async obj =>
                {
                    if (isConnected)
                    {
                        return;
                    }

                    try
                    {
                        var startServer = new Thread(async () =>
                        {
                            if (server == null)
                            {
                                server = new Server(port);
                                await server.Start();
                            }
                        });
                        startServer.Start();

                        client = new Client(hostname, port);
                        client.Connect();
                        isConnected = true;
                        await UpdateServerPath("..\\..");
                    }
                    catch (Exception exception)
                    {
                        HandleException(exception);
                        isConnected = false;
                    }
                }, (obj) => hostname != "" && port != -1);
            }
        }

        private async Task UpdateServerPath(string path)
        {
            while (ServerFolderList.Count > 0)
            {
                ServerFolderList.RemoveAt(ServerFolderList.Count - 1);
            }

            var serverList = await client.List(path);

            foreach (var item in serverList)
            {
                var name = item.Item1;
                ServerFolderList.Add(name.Substring(name.LastIndexOf('\\') + 1));
            }
            serverPath = path;
        }
    }
}
