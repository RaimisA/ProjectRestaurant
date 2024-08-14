using ProjectRestaurant.Enums;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;
        private readonly string _filePath;

        public OrderRepository(string filePath)
        {
            _filePath = filePath;
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _orders = JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
            }
            else
            {
                _orders = new List<Order>();
            }
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            SaveOrdersToFile();
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        public void MarkOrderInProgress(int tableNumber)
        {
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && o.Status == OrderStatus.New);
            if (order != null)
            {
                order.Status = OrderStatus.InProgress;
                SaveOrdersToFile();
            }
        }

        public void MarkOrderComplete(int tableNumber)
        {
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && o.Status == OrderStatus.InProgress);
            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                SaveOrdersToFile();
            }
        }

        public void MarkOrderCanceled(int tableNumber)
        {
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && o.Status == OrderStatus.InProgress);
            if (order != null)
            {
                order.Status = OrderStatus.Canceled;
                SaveOrdersToFile();
            }
        }
        private void SaveOrdersToFile()
        {
            var json = JsonSerializer.Serialize(_orders);
            File.WriteAllText(_filePath, json);
        }
    }
}
