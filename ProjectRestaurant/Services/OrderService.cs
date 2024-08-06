using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TableRepository _tableRepository;

        public OrderService(OrderRepository orderRepository, TableRepository tableRepository)
        {
            _orderRepository = orderRepository;
            _tableRepository = tableRepository;
        }

        public void PlaceOrder(Order order)
        {
            _orderRepository.AddOrder(order);
            _tableRepository.MarkTableAsOccupied(order.Table.TableNumber);
        }

        public List<Order> GetOrders()
        {
            return _orderRepository.GetAllOrders();
        }
    }
}
