using ProjectRestaurant.Models;

namespace ProjectRestaurant.Services
{
    public class CheckService
    {
        public void PrintCheck(Order order, bool isClientCheck)
        {
            // Implementation for printing check
        }

        public void SaveCheckToFile(Order order, string filePath)
        {
            File.WriteAllText(filePath, order.ToString());
        }
    }
}
