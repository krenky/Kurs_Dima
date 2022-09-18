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
    public class ListOperationTests
    {
        [TestMethod()]
        public void AddOperationTest()
        {
            //Arrange
            int[] operation = { 1, 2, 3, -3, -2, -1 };
            ListOperation listOperation = new ListOperation();

            //Act
            foreach (var oper in operation)
                listOperation.AddOperation(oper);

            //Assert
            //Assert.AreEqual(listOperation.FirstOperation.Amount, operation[0]);
        }

        [TestMethod()]
        public void SumOperationTest()
        {
            //Arrange
            int[] operation = { 1, 2, 3, -3, -2, -1 };
            ListOperation listOperation = new ListOperation();

            //Act
            foreach (var oper in operation)
                listOperation.AddOperation(oper);

            //Assert
            Assert.AreEqual(listOperation.SumAmount, 0);
        }

        [TestMethod()]
        public void AddBeforeOperationTest()
        {
            //Arrange
            int[] operation = { 1, 2, 3, -3, -2, -1 };
            ListOperation listOperation = new ListOperation();

            //Act
            foreach (var oper in operation)
                listOperation.AddBeforeOperation(oper, 0);

            //Assert
            Assert.AreEqual(listOperation.SumAmount, 0);
        }
        [TestMethod()]
        public void AddBeforeOperationTest1()
        {
            //Arrange
            int[] operation = { 1, 2, 3, -3, -2, -1 };
            ListOperation listOperation = new ListOperation();

            //Act
            foreach (var oper in operation)
                listOperation.AddBeforeOperation(oper, 0);

            //Assert
            //Assert.AreEqual(listOperation.FirstOperation.Previous.Amount, -1);
        }

        [TestMethod()]
        public void AddAfterOperationTest()
        {
            //Arrange
            int[] operation = { 1, 2, 3, -3, -2, -1 };
            ListOperation listOperation = new ListOperation();

            //Act
            foreach (var oper in operation)
                listOperation.AddAfterOperation(oper, 0);

            //Assert
            //Assert.AreEqual(listOperation.FirstOperation.Next.Amount, -1);
        }
    }
}