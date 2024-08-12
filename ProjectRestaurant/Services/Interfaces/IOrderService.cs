using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Interfaces
{
    public interface IOrderService
    {
        Order? PlaceOrder();
        void ViewOrders();
        Order? GetOrderForTable(int tableNumber);
        void ShowOrderForTable(int tableNumber);
        void MarkOrderAsCompleted(int tableNumber);
        void MarkOrderAsCanceled(int tableNumber);
    }
}
