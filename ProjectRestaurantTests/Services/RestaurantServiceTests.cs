using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Enums;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Repositories.Interfaces;
using ProjectRestaurant.Services;
using ProjectRestaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Tests
{
    [TestClass]
    public class RestaurantServiceTests
    {
        private TableRepository? _tableRepository;
        private OrderRepository? _orderRepository;
        private OrderService? _orderService;
        private CheckService? _checkService;
        private EmailService? _emailService;
        private RestaurantService? _restaurantService;

        [TestInitialize]
        public void Setup()
        {
            _tableRepository = new TableRepository("tables.csv");
            _orderRepository = new OrderRepository("empty_orders.json"); // Use an empty data source for testing
            var itemRepository = new ItemRepository("food.csv", "drinks.csv");
            _orderService = new OrderService(_orderRepository, itemRepository, _tableRepository);
            var checkRepository = new CheckRepository();
            _checkService = new CheckService(checkRepository);
            _emailService = new EmailService();

            _restaurantService = new RestaurantService(
                _tableRepository,
                _orderRepository,
                _orderService,
                _checkService,
                _emailService,
                "restaurantCheckFilePath",
                "clientCheckBasePath"
            );

            // Ensure tables are available
            _tableRepository.MarkTableAsAvailable(1);
        }

        [TestMethod]
        public void GetOrders_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Table = _tableRepository!.GetAllTables().First(), Status = OrderStatus.InProgress },
                new Order { Table = _tableRepository.GetAllTables().First(), Status = OrderStatus.InProgress }
            };
            foreach (var order in orders)
            {
                _orderRepository!.AddOrder(order);
            }

            // Act
            var result = _restaurantService!.GetOrders();

            // Assert
            Assert.AreEqual(orders.Count, result.Count);
            CollectionAssert.AreEqual(orders, result);
        }
    }
}