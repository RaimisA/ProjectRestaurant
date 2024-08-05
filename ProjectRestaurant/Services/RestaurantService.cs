using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services
{
    public class Restaurant
    {
        private readonly TableRepository _tableRepository;
        private readonly OrderRepository _orderRepository;
        private readonly OrderService _orderService;
        private readonly CheckService _checkService;
        private readonly IEmailService _emailService;

        public Restaurant(TableRepository tableRepository, OrderRepository orderRepository, OrderService orderService, CheckService checkService, IEmailService emailService)
        {
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
            _orderService = orderService;
            _checkService = checkService;
            _emailService = emailService;
        }

        public void RegisterOrder(Order order)
        {
            _orderService.PlaceOrder(order);
            _checkService.PrintCheck(order, isClientCheck: true);
            _checkService.SaveCheckToFile(order, "restaurant_check.txt");

            // Send emails
            _emailService.SendEmail("client@example.com", "Your Order", "Order details...");
            _emailService.SendEmail("restaurant@example.com", "New Order", "Order details...");
        }

        public List<Table> GetTables()
        {
            return _tableRepository.GetAllTables();
        }

        public List<Order> GetOrders()
        {
            return _orderRepository.GetAllOrders();
        }
    }
}
