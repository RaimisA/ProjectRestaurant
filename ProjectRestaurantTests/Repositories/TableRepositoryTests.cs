using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories.Tests
{
    [TestClass]
    public class TableRepositoryTests
    {
        private string _filePath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _filePath = Path.GetTempFileName();
            File.WriteAllLines(_filePath, new string[]
            {
                "1,4,False",
                "2,4,False",
                "3,2,False",
                "4,6,False"
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [TestMethod]
        public void TableRepositoryTest()
        {
            // Arrange & Act
            var tableRepository = new TableRepository(_filePath);

            // Assert
            Assert.IsNotNull(tableRepository);
            Assert.AreEqual(4, tableRepository.GetAllTables().Count);
        }

        [TestMethod]
        public void GetAllTablesTest()
        {
            // Arrange
            var tableRepository = new TableRepository(_filePath);

            // Act
            var tables = tableRepository.GetAllTables();

            // Assert
            Assert.AreEqual(4, tables.Count);
        }

        [TestMethod]
        public void GetTableByNumberTest()
        {
            // Arrange
            var tableRepository = new TableRepository(_filePath);

            // Act
            var table = tableRepository.GetTableByNumber(1);

            // Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(1, table?.TableNumber);
            Assert.AreEqual(4, table?.Seats);
            Assert.IsFalse(table?.IsOccupied);
        }

        [TestMethod]
        public void MarkTableAsOccupiedTest()
        {
            // Arrange
            var tableRepository = new TableRepository(_filePath);

            // Act
            tableRepository.MarkTableAsOccupied(1);
            var table = tableRepository.GetTableByNumber(1);

            // Assert
            Assert.IsNotNull(table);
            Assert.IsTrue(table?.IsOccupied);
        }

        [TestMethod]
        public void MarkTableAsAvailableTest()
        {
            // Arrange
            var tableRepository = new TableRepository(_filePath);
            tableRepository.MarkTableAsOccupied(1);

            // Act
            tableRepository.MarkTableAsAvailable(1);
            var table = tableRepository.GetTableByNumber(1);

            // Assert
            Assert.IsNotNull(table);
            Assert.IsFalse(table?.IsOccupied);
        }
    }
}