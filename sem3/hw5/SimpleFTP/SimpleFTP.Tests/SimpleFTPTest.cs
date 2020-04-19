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

            server.Start();
            client.Connect();
        }

        private void Stop()
        {
            server.Stop();
            client.Stop();
            client.Dispose();
        }

        [TestMethod]
        public async Task ListTest()
        {
            var response = await client.List(path);
            Assert.AreEqual(3, response.Count);
            Assert.IsTrue(response.Contains((".\\testfile.txt", false)));
            Assert.IsTrue(response.Contains((".\\TestDirectory1", true)));
            Assert.IsTrue(response.Contains((".\\TestDirectory2", true)));
            Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public async Task DirectoryDoesNotExistListTest()
        {
            _ = await client.List(path + "\\NonexistentDirectory");
            Stop();
        }

        [TestMethod]
        public async Task GetTest()
        {
            var response = await client.Get(filePath);
            var file = File.ReadAllBytes(filePath);
            var expected = $"{file.Length} {file}";
            Assert.AreEqual(expected, response);
            Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileDoesNotExistGetTest()
        {
            _ = await client.Get(path + "\\NonexistentFile.txt");
            Stop();
        }

        private Client client;
        private Server server;
        private readonly string path = "..\\..\\..\\SimpleFTP.Tests\\TestFiles";
        private readonly string filePath = "..\\..\\..\\SimpleFTP.Tests\\TestFiles\\testfile.txt";
    }
}
