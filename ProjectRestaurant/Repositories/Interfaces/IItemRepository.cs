using ProjectRestaurant.Models;

namespace ProjectRestaurant.Repositories.Interfaces
{
    public interface IItemRepository
    {
        List<Item> GetAllItems();
        List<Item> GetDrinkItems();
        List<Item> GetFoodItems();
    }
}