using Microsoft.VisualStudio.TestTools.UnitTesting;
using FTPServer;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GUIForFTP.Tests
{
    [TestClass]
    public class GUIForFTPTest
    {
        private string hostname;
        private int port;
        private ApplicationViewModel viewModel;

        [TestInitialize]
        public void Initialize()
        {
            hostname = "localhost";
            port = 1234;

            var startServer = new Thread(async () =>
            {
                var server = new Server(port);
                await server.Start();
            });
            startServer.Start();

            viewModel = new ApplicationViewModel(hostname, port);
        }

        [TestMethod]
        public async Task ConnectTest()
        {
            Assert.IsFalse(viewModel.IsConnected);

            await viewModel.Connect();

            Assert.IsTrue(viewModel.IsConnected);
            Assert.IsTrue(viewModel.DisplayedServerFolderList.Count != 0);
        }

        [TestMethod]
        public async Task OpenServerFolderTest()
        {
            await viewModel.Connect();
            await viewModel.OpenOrDownloadServerItem("TestFiles");

            Assert.AreEqual("/TestFiles", viewModel.ServerPath);
            CheckTestFilesFolder(viewModel.DisplayedServerFolderList);

            await viewModel.OpenOrDownloadServerItem("TestFolder");

            Assert.AreEqual("/TestFiles/TestFolder", viewModel.ServerPath);
            CheckTestFolder(viewModel.DisplayedServerFolderList);
        }

        [TestMethod]
        public async Task ServerFolderUpTest()
        {
            await viewModel.Connect();
            var rootServerFolder = viewModel.DisplayedServerFolderList;

            await viewModel.OpenOrDownloadServerItem("TestFiles");
            await viewModel.OpenOrDownloadServerItem("TestFolder");

            Assert.AreEqual("/TestFiles/TestFolder", viewModel.ServerPath);
            CheckTestFolder(viewModel.DisplayedServerFolderList);

            await viewModel.GoServerFolderUp();

            Assert.AreEqual("/TestFiles", viewModel.ServerPath);
            CheckTestFilesFolder(viewModel.DisplayedServerFolderList);

            await viewModel.GoServerFolderUp();

            CollectionAssert.AreEqual(rootServerFolder, viewModel.DisplayedServerFolderList);
            Assert.AreEqual("", viewModel.ServerPath);
        }

        [TestMethod]
        public async Task OpenClientFolderTest()
        {
            await viewModel.Connect();
            var path = Directory.GetCurrentDirectory() + "/../../../TestFiles";

            viewModel.ChooseClientFolder(path);
            Assert.AreEqual(path, viewModel.ClientPath);
            CheckTestFilesFolder(viewModel.DisplayedClientFolderList);

            viewModel.OpenClientFolder("TestFolder");
            Assert.AreEqual(path + "/TestFolder", viewModel.ClientPath);
            CheckTestFolder(viewModel.DisplayedClientFolderList);
        }

        [TestMethod]
        public async Task ClientFolderUpTest()
        {
            await viewModel.Connect();
            var path = Directory.GetCurrentDirectory() + "/../../../TestFiles";

            viewModel.ChooseClientFolder(path);
            viewModel.OpenClientFolder("TestFolder");

            Assert.AreEqual(path + "/TestFolder", viewModel.ClientPath);
            CheckTestFolder(viewModel.DisplayedClientFolderList);

            viewModel.GoClientFolderUp();

            Assert.AreEqual(path, viewModel.ClientPath);
            CheckTestFilesFolder(viewModel.DisplayedClientFolderList);
        }

        private void CheckTestFilesFolder(ObservableCollection<string> collection)
        {
            Assert.IsTrue(collection.Count == 4);
            Assert.IsTrue(collection.Contains("TestFolder"));
            Assert.IsTrue(collection.Contains("TestFile1.txt"));
            Assert.IsTrue(collection.Contains("TestFile2.txt"));
            Assert.IsTrue(collection.Contains("TestFile3.txt"));
        }

        private void CheckTestFolder(ObservableCollection<string> collection)
        {
            Assert.IsTrue(collection.Count == 1);
            Assert.IsTrue(collection.Contains("TestFile4.txt"));
        }
    }
}
