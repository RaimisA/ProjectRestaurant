using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Enums;
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
    public class OrderRepositoryTests
    {
        private string _filePath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _filePath = Path.GetTempFileName();
            // Initialize the file with an empty JSON array
            File.WriteAllText(_filePath, "[]");
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
        public void OrderRepositoryTest()
        {
            // Arrange & Act
            var orderRepository = new OrderRepository(_filePath);

            // Assert
            Assert.IsNotNull(orderRepository);
            Assert.AreEqual(0, orderRepository.GetAllOrders().Count);
        }

        [TestMethod]
        public void AddOrderTest()
        {
            // Arrange
            var orderRepository = new OrderRepository(_filePath);
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
            orderRepository.AddOrder(order);

            // Assert
            var orders = orderRepository.GetAllOrders();
            Assert.AreEqual(1, orders.Count);
            Assert.IsNotNull(orders[0].Table);
            Assert.AreEqual(1, orders[0].Table?.TableNumber); // Added null check
        }

        [TestMethod]
        public void GetAllOrdersTest()
        {
            // Arrange
            var orderRepository = new OrderRepository(_filePath);
            var order1 = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                }
            };
            var order2 = new Order
            {
                Table = new Table { TableNumber = 2 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Burger", Price = 8.0m }, Quantity = 1 }
                }
            };
            orderRepository.AddOrder(order1);
            orderRepository.AddOrder(order2);

            // Act
            var orders = orderRepository.GetAllOrders();

            // Assert
            Assert.AreEqual(2, orders.Count);
        }

        [TestMethod]
        public void MarkOrderInProgressTest()
        {
            // Arrange
            var orderRepository = new OrderRepository(_filePath);
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                },
                Status = OrderStatus.New
            };
            orderRepository.AddOrder(order);

            // Act
            orderRepository.MarkOrderInProgress(1);

            // Assert
            var orders = orderRepository.GetAllOrders();
            Assert.AreEqual(OrderStatus.InProgress, orders[0].Status);
        }

        [TestMethod]
        public void MarkOrderCompleteTest()
        {
            // Arrange
            var orderRepository = new OrderRepository(_filePath);
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                },
                Status = OrderStatus.InProgress
            };
            orderRepository.AddOrder(order);

            // Act
            orderRepository.MarkOrderComplete(1);

            // Assert
            var orders = orderRepository.GetAllOrders();
            Assert.AreEqual(OrderStatus.Completed, orders[0].Status);
        }

        [TestMethod]
        public void MarkOrderCanceledTest()
        {
            // Arrange
            var orderRepository = new OrderRepository(_filePath);
            var order = new Order
            {
                Table = new Table { TableNumber = 1 },
                DateTime = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem { Item = new Item { Name = "Pizza", Price = 10.0m }, Quantity = 2 }
                },
                Status = OrderStatus.InProgress
            };
            orderRepository.AddOrder(order);

            // Act
            orderRepository.MarkOrderCanceled(1);

            // Assert
            var orders = orderRepository.GetAllOrders();
            Assert.AreEqual(OrderStatus.Canceled, orders[0].Status);
        }
    }
}