using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Enums;

namespace ProjectRestaurant.Services.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private OrderRepository? _orderRepository;
        private ItemRepository? _itemRepository;
        private TableRepository? _tableRepository;
        private OrderService? _orderService;

        [TestInitialize]
        public void Setup()
        {
            // Provide file paths for the repositories
            _orderRepository = new OrderRepository("orders.json");
            _itemRepository = new ItemRepository("food.csv", "drinks.csv");
            _tableRepository = new TableRepository("tables.csv");
            _orderService = new OrderService(_orderRepository, _itemRepository, _tableRepository);

            // Ensure tables are available
            _tableRepository.MarkTableAsAvailable(1);

            // Ensure items are loaded correctly
            var foodItems = _itemRepository.GetFoodItems();
            var drinkItems = _itemRepository.GetDrinkItems();
        }

        [TestMethod]
        public void GetOrderForTableTest()
        {
            // Arrange
            var tableNumber = 1;
            var order = new Order
            {
                Table = _tableRepository!.GetAllTables().First(t => t.TableNumber == tableNumber),
                Status = OrderStatus.InProgress
            };
            _orderRepository!.AddOrder(order);

            // Act
            var result = _orderService!.GetOrderForTable(tableNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Table);
            Assert.AreEqual(tableNumber, result.Table.TableNumber);
        }

        [TestMethod]
        public void ShowOrderForTableTest()
        {
            // Arrange
            var tableNumber = 1;
            var order = new Order
            {
                Table = _tableRepository!.GetAllTables().First(t => t.TableNumber == tableNumber),
                DateTime = DateTime.Now,
                Items = new List<OrderItem>()
            };
            _orderRepository!.AddOrder(order);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                _orderService!.ShowOrderForTable(tableNumber);

                // Assert
                var result = sw.ToString();
                Assert.IsTrue(result.Contains($"Order for Table {tableNumber}"));
            }
        }

        [TestMethod]
        public void MarkOrderAsCanceledTest()
        {
            // Arrange
            var tableNumber = 1;
            var order = new Order
            {
                Table = _tableRepository!.GetAllTables().First(t => t.TableNumber == tableNumber),
                Status = OrderStatus.InProgress
            };
            _orderRepository!.AddOrder(order);

            // Act
            _orderService!.MarkOrderAsCanceled(tableNumber);

            // Assert
            var canceledOrder = _orderRepository.GetAllOrders().FirstOrDefault(o => o.Table.TableNumber == tableNumber);
            Assert.IsNotNull(canceledOrder);
            if (canceledOrder != null)
            {
                Assert.AreEqual(OrderStatus.Canceled, canceledOrder.Status);
            }
        }
    }
}