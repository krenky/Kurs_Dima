using Microsoft.VisualStudio.TestTools.UnitTesting;
using Class_library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_library.Tests
{
    [TestClass()]
    public class QueueClientTests
    {
        [TestMethod()]
        public void AddClientTest()
        {
            //Arrange
            string[] clients = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            QueueClient queueClient = new(10);

            //Act
            foreach (string client in clients)
            {
                queueClient.AddClient(client);
            }

            //Asaert
            Assert.AreEqual(queueClient.Clients[0].Name, "11");

        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Arrange
            string[] clients = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            QueueClient queueClient = new(10);

            //Act
            foreach (string client in clients)
            {
                queueClient.AddClient(client);
            }
            queueClient.Delete();
            //Assert
            bool bl = true;
            bl = queueClient.CountClient == 9 && queueClient.GetIndexFirstClient() == 3;
            Assert.IsTrue(bl);
            Assert.AreEqual(queueClient.Clients[2], null);
        }
    }
}