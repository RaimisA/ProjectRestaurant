using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories
{
    public class OrderRepository
    {
        private List<Order> orders = new List<Order>();

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public List<Order> GetAllOrders()
        {
            return orders;
        }
    }
}
