using ProjectRestaurant.Models;

namespace ProjectRestaurant.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        List<Order> GetAllOrders();
        void MarkOrderCanceled(int tableNumber);
        void MarkOrderComplete(int tableNumber);
        void MarkOrderInProgress(int tableNumber);
    }
}