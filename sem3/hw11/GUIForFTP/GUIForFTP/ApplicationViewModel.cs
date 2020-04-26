using SimpleFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace GUIForFTP
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string hostname;
        private int port;
        private string serverPath;
        private string clientPath;
        private Client client;
        private Server server;
        private bool isConnected = false;
        private string serverRoot;
        private List<(string, bool)> serverFolderList = new List<(string, bool)>();
        private List<(string, bool)> clientFolderList = new List<(string, bool)>();

        public ObservableCollection<string> DisplayedServerFolderList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> DisplayedClientFolderList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> InProcessList { get; private set; } 
            = new ObservableCollection<string>();

        public ObservableCollection<string> DownloadingList { get; private set; } 
            = new ObservableCollection<string>();

        public string ServerPath
        {
            get
                => serverPath.Substring(8);
            set
            {
                serverPath = value;
                NotifyPropertyChanged();
            }
        }

        public string ClientPath
        {
            get
                => clientPath;
            set
            {
                clientPath = value;
                NotifyPropertyChanged();
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
            serverRoot = "../../..";
            serverPath = serverRoot;
            clientPath = "Choose folder";
        }

        public void NotifyPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void HandleException(Exception exception)
        {
            System.Windows.MessageBox.Show("An exception just occurred: " + exception.Message,
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
                        await UpdateServerList(serverRoot);
                    }
                    catch (Exception exception)
                    {
                        HandleException(exception);
                        serverFolderList.Clear();
                        DisplayedServerFolderList.Clear();
                        isConnected = false;
                    }
                }, obj => hostname != "" && port != -1);
            }
        }

        public ICommand GoServerFolderUp
        {
            get
            {
                return new Command(async obj =>
                {
                    await UpdateServerList(serverPath.Substring(0, serverPath.LastIndexOf('/')));
                }, obj => serverPath != serverRoot);
            }
        }

        public ICommand Help
        {
            get
            {
                return new Command(obj =>
                {
                    System.Windows.MessageBox.Show("Enter server's hostname and port and click 'Connect' button to connect to the server.\n" +
                        "Click 'Choose folder' button to choose a client's file system folder for downloading files.\n" +
                        "Double click on folder to open it; click on '..' button to go folder up.\n" +
                        "Choose file and click 'Download' button or double click on the file to download it into the client's downloads folder.\n" +
                        "Click 'Download all' button to download all files from the current server path into the client's downloads folder.\n",
                "Help",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                });
            }
        }

        public ICommand ChooseDownloadFolder
        {
            get
            {
                return new Command(obj =>
                {
                    FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                    if (folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        ClientPath = folderBrowser.SelectedPath.Replace('\\', '/');
                        UpdateClientList(clientPath);
                    }
                });
            }
        }

        private void UpdateClientList(string path)
        {
            clientFolderList.Clear();

            var directories = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);
            
            foreach (var directory in directories)
            {
                clientFolderList.Add((directory, true));
            }

            foreach (var file in files)
            {
                clientFolderList.Add((file, true));
            }

            UpdateDisplayedClientFolderList();
        }

        private void UpdateDisplayedClientFolderList()
        {
            DisplayedClientFolderList.Clear();

            foreach (var item in clientFolderList)
            {
                var name = item.Item1;
                DisplayedClientFolderList.Add(name.Substring(name.LastIndexOf('\\') + 1));
            }
        }

        public void OpenClientFolder(string name)
        {
            foreach (var item in clientFolderList)
            {
                if (item.Item1.EndsWith(name))
                {
                    if (item.Item2)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }    
                }
            }

            UpdateClientList(clientPath + $"/{name}");
        }

        private async Task UpdateServerList(string path)
        {
            serverFolderList.Clear();
            serverFolderList = await client.List(path);
            UpdateDisplayedServerFolderList();
            ServerPath = path;
        }

        private void UpdateDisplayedServerFolderList()
        {
            DisplayedServerFolderList.Clear();
            foreach (var item in serverFolderList)
            {
                var name = item.Item1;
                DisplayedServerFolderList.Add(name.Substring(name.LastIndexOf('\\') + 1));
            }
        }

        private bool IsDirectory(string name)
        {
            foreach (var item in serverFolderList)
            {
                if (item.Item1.EndsWith(name))
                {
                    return item.Item2;
                }
            }
            return false;
        }

        public async Task OpenOrDownloadServerItem(string name)
        {
            if (IsDirectory(name))
            {
                await UpdateServerList(serverPath + $"/{name}");
            }
            else
            {
                //Скачиваем файл.
            }
        }
    }
}
