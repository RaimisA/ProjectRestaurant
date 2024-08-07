using ProjectRestaurant.Models;
using System.Globalization;

namespace ProjectRestaurant.Repositories
{
    public class ItemRepository
    {
        private readonly string _foodFilePath;
        private readonly string _drinkFilePath;

        public ItemRepository(string foodFilePath, string drinkFilePath)
        {
            _foodFilePath = foodFilePath;
            _drinkFilePath = drinkFilePath;
        }

        public List<Item> GetFoodItems()
        {
            return ReadItemsFromFile(_foodFilePath);
        }

        public List<Item> GetDrinkItems()
        {
            return ReadItemsFromFile(_drinkFilePath);
        }

        private List<Item> ReadItemsFromFile(string filePath)
        {
            var items = new List<Item>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && decimal.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                    {
                        items.Add(new Item { Name = parts[0], Price = price });
                    }
                }
            }

            return items;
        }

        public List<Item> GetAllItems()
        {
            var foodItems = GetFoodItems();
            var drinkItems = GetDrinkItems();
            return foodItems.Concat(drinkItems).ToList();
        }
    }
}
