using ProjectRestaurant.Models;

namespace ProjectRestaurant.Repositories.Interfaces
{
    public interface ITableRepository
    {
        List<Table> GetAllTables();
        Table? GetTableByNumber(int tableNumber);
        void MarkTableAsAvailable(int tableNumber);
        void MarkTableAsOccupied(int tableNumber);
    }
}