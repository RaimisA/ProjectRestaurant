using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories
{
    public class OrderRepository
    {
        private readonly string _ordersFilePath;

        public OrderRepository(string ordersFilePath)
        {
            _ordersFilePath = ordersFilePath;
        }

        public List<Order> GetAllOrders()
        {
            if (!File.Exists(_ordersFilePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(_ordersFilePath);
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        public void AddOrder(Order order)
        {
            var orders = GetAllOrders();
            orders.Add(order);
            SaveOrdersToFile(orders);
        }

        private void SaveOrdersToFile(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_ordersFilePath, json);
        }
    }
}
