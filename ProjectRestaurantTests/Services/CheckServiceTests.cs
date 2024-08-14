using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Tests
{
    [TestClass]
    public class CheckServiceTests
    {
        private CheckService? _checkService;
        private CheckRepository? _checkRepository;

        [TestInitialize]
        public void Setup()
        {
            _checkRepository = new CheckRepository();
            _checkService = new CheckService(_checkRepository);
        }

        [TestMethod]
        public void PrintClientCheckTest()
        {
            // Arrange
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _checkService!.PrintCheck(order, true);
                var result = sw.ToString();

                // Log the actual result for debugging
                Console.WriteLine("Actual Output:");
                Console.WriteLine(result);

                // Assert
                Assert.IsTrue(result.Contains("Client Check"), "Client Check header is missing");
                Assert.IsTrue(result.Contains("Pizza                x2 - 20,00 €"), $"Order item details are missing or incorrect. Actual output: {result}");
                Assert.IsTrue(result.Contains("Total: 20,00 €"), $"Total amount is missing or incorrect. Actual output: {result}");
                Assert.IsTrue(result.Contains("Thank you for dining with us!"), $"Thank you message is missing. Actual output: {result}");
            }
        }

        [TestMethod]
        public void PrintRestaurantCheckTest()
        {
            // Arrange
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _checkService!.PrintCheck(order, false);
                var result = sw.ToString();

                // Log the actual result for debugging
                Console.WriteLine("Actual Output:");
                Console.WriteLine(result);

                // Assert
                Assert.IsTrue(result.Contains("Restaurant Check"), "Restaurant Check header is missing");
                Assert.IsTrue(result.Contains("Pizza                x2 - 20,00 €"), $"Order item details are missing or incorrect. Actual output: {result}");
                Assert.IsTrue(result.Contains("Total: 20,00 €"), $"Total amount is missing or incorrect. Actual output: {result}");
                Assert.IsTrue(result.Contains("Table: 1"), $"Table number is missing or incorrect. Actual output: {result}");
                Assert.IsTrue(result.Contains("Internal Notes for restaurant: Ensure inventory is updated."), $"Internal notes are missing or incorrect. Actual output: {result}");
            }
        }

        [TestMethod]
        public void SaveClientCheckToFileTest()
        {
            // Arrange
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };
            var filePath = Path.GetTempFileName();

            // Act
            _checkService!.SaveCheckToFile(order, filePath, true);

            // Assert
            var fileContent = File.ReadAllText(filePath);
            Assert.IsTrue(fileContent.Contains("Client Check"), "Client Check header is missing");
            Assert.IsTrue(fileContent.Contains("Pizza                x2 - 20,00 €"), $"Order item details are missing or incorrect. Actual output: {fileContent}");
            Assert.IsTrue(fileContent.Contains("Total: 20,00 €"), $"Total amount is missing or incorrect. Actual output: {fileContent}");
            Assert.IsTrue(fileContent.Contains("Thank you for dining with us!"), $"Thank you message is missing. Actual output: {fileContent}");

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveRestaurantCheckToFileTest()
        {
            // Arrange
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };
            var filePath = Path.GetTempFileName();

            // Act
            _checkService!.SaveCheckToFile(order, filePath, false);

            // Assert
            var fileContent = File.ReadAllText(filePath);
            Assert.IsTrue(fileContent.Contains("Restaurant Check"), "Restaurant Check header is missing");
            Assert.IsTrue(fileContent.Contains("Pizza                x2 - 20,00 €"), $"Order item details are missing or incorrect. Actual output: {fileContent}");
            Assert.IsTrue(fileContent.Contains("Total: 20,00 €"), $"Total amount is missing or incorrect. Actual output: {fileContent}");
            Assert.IsTrue(fileContent.Contains("Table: 1"), $"Table number is missing or incorrect. Actual output: {fileContent}");
            Assert.IsTrue(fileContent.Contains("Internal Notes for restaurant: Ensure inventory is updated."), $"Internal notes are missing or incorrect. Actual output: {fileContent}");

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveClientCheckToFile_ShouldContainEuroSymbol()
        {
            // Arrange
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };
            var filePath = Path.GetTempFileName();

            // Act
            _checkService!.SaveCheckToFile(order, filePath, isClientCheck: true);

            // Assert
            var fileContent = File.ReadAllText(filePath);
            Assert.IsTrue(fileContent.Contains("€"), "The file content should contain the euro symbol.");

            // Clean up
            File.Delete(filePath);
        }
    }
}