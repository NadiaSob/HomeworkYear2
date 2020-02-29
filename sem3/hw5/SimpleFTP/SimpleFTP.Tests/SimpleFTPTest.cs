using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTP;
using System.IO;

namespace SimpleFTP.Tests
{
    [TestClass]
    public class SimpleFTPTest
    {
        [TestInitialize]
        public void Initialize()
        {
            server = new Server(1234);
            client = new Client("localhost", 1234);
        }

        private void Start()
        {
            server.Start();
            client.Connect();
        }

        private void Stop()
        {
            server.Stop();
            client.Stop();
        }

        [TestMethod]
        public async Task ListTest()
        {
            Start();
            var response = await client.List(path);
            var expected = "3 .\\testfile.txt false .\\TestDirectory1 true .\\TestDirectory2 true ";
            Assert.AreEqual(expected, response);
            Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public async Task DirectoryDoNotExistListTest()
        {
            Start();
            _ = await client.List(path + "\\NonexistentDirectory");
            Stop();
        }

        [TestMethod]
        public async Task GetTest()
        {
            Start();
            var response = await client.Get(filePath);
            var file = File.ReadAllBytes(filePath);
            var expected = $"{file.Length} {file}";
            Assert.AreEqual(expected, response);
            Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileDoNotExistListTest()
        {
            Start();
            _ = await client.Get(path + "\\NonexistentFile.txt");
            Stop();
        }

        private Client client;
        private Server server;
        private readonly string path = "..\\..\\..\\SimpleFTP.Tests\\TestFiles";
        private readonly string filePath = "..\\..\\..\\SimpleFTP.Tests\\TestFiles\\testfile.txt";
    }
}
