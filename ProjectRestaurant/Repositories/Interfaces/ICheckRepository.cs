using ProjectRestaurant.Models;

namespace ProjectRestaurant.Repositories.Interfaces
{
    public interface ICheckRepository
    {
        void SaveCheckToFile(Order order, string filePath, bool isClientCheck);
    }
}