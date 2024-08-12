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
            EnsureFilesExist();
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

        private void EnsureFilesExist()
        {
            CreateFileIfNotFound(_foodFilePath, new string[]
            {
                "Beef Burger,10.59",
                "Steak,12.99",
                "Tuna tartar,8.50",
                "Pasta,7.25",
                "Pizza,8.99"
            });

            CreateFileIfNotFound(_drinkFilePath, new string[]
            {
                "Pepsi,1.99",
                "Water,0.99",
                "Beer,3.50",
                "Wine,5.00",
                "Juice,2.50",
                "Coffee,1.50",
                "Tea,1.25"
            });
        }

        private void CreateFileIfNotFound(string filePath, string[] content)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllLines(filePath, content);
                Console.WriteLine($"{filePath} created.");
            }
            else
            {
                Console.WriteLine($"{filePath} already exists.");
            }
        }
    }
}
