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
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && !o.IsCompleted && !o.IsCanceled);
            if (order != null)
            {
                order.IsInProgress = true;
                SaveOrdersToFile();
            }
        }

        public void MarkOrderComplete(int tableNumber)
        {
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && o.IsInProgress);
            if (order != null)
            {
                order.IsInProgress = false;
                order.IsCompleted = true;
                SaveOrdersToFile();
            }
        }

        public void MarkOrderCanceled(int tableNumber)
        {
            var order = _orders.FirstOrDefault(o => o.Table != null && o.Table.TableNumber == tableNumber && o.IsInProgress);
            if (order != null)
            {
                order.IsInProgress = false;
                order.IsCanceled = true;
                SaveOrdersToFile();
            }
        }

        //public void UpdateOrder(Order updatedOrder)
        //{
        //    var order = _orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
        //    if (order != null)
        //    {
        //        order.Table = updatedOrder.Table;
        //        order.Client = updatedOrder.Client;
        //        order.OrderItems = updatedOrder.OrderItems;
        //        order.OrderDateTime = updatedOrder.OrderDateTime;
        //        order.IsInProgress = updatedOrder.IsInProgress;
        //        order.IsCompleted = updatedOrder.IsCompleted;
        //        order.IsCanceled = updatedOrder.IsCanceled;
        //        SaveOrdersToFile();
        //    }
        //}

        private void SaveOrdersToFile()
        {
            var json = JsonSerializer.Serialize(_orders);
            File.WriteAllText(_filePath, json);
        }
    }
}
