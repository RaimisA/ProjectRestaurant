using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Interfaces
{
    public interface IRestaurantService
    {
        void RegisterOrder();
        void CompleteOrder();
        List<Order> GetOrders();
    }
}
