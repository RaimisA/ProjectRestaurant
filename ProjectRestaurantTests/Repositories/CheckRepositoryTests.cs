using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories.Tests
{
    [TestClass]
    public class CheckRepositoryTests
    {
        [TestMethod]
        public void SaveCheckToFile_ShouldSaveClientCheck()
        {
            // Arrange
            var checkRepository = new CheckRepository();
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 },
                    new OrderItem { Item = new Item { Name = "Coke", Price = 5.0m }, Quantity = 2 }
                }
            };
            var filePath = Path.GetTempFileName();

            // Act
            checkRepository.SaveCheckToFile(order, filePath, true);

            // Assert
            var fileContent = File.ReadAllText(filePath);
            Assert.IsTrue(fileContent.Contains("This is a client check."));
            Assert.IsTrue(fileContent.Contains("Order for Table 1"));
            Assert.IsTrue(fileContent.Contains("Pizza x 2 = 20.00 EUR"));
            Assert.IsTrue(fileContent.Contains("Coke x 2 = 10.00 EUR"));

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveCheckToFile_ShouldSaveRestaurantCheck()
        {
            // Arrange
            var checkRepository = new CheckRepository();
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 },
                    new OrderItem { Item = new Item { Name = "Coke", Price = 5.0m }, Quantity = 2 }
                }
            };
            var filePath = Path.GetTempFileName();

            // Act
            checkRepository.SaveCheckToFile(order, filePath, false);

            // Assert
            var fileContent = File.ReadAllText(filePath);
            Assert.IsTrue(fileContent.Contains("This is a restaurant check."));
            Assert.IsTrue(fileContent.Contains("Order for Table 1"));
            Assert.IsTrue(fileContent.Contains("Pizza x 2 = 20.00 EUR"));
            Assert.IsTrue(fileContent.Contains("Coke x 2 = 10.00 EUR"));

            // Clean up
            File.Delete(filePath);
        }
    }
}