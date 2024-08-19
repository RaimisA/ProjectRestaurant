using System.Reflection.Metadata;
using ProjectRestaurant.Services;
using ProjectRestaurant.Models;
using ProjectRestaurant.Services.Interfaces;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Presentation;
using System.Text;
using ProjectRestaurant.Repositories.Interfaces;

namespace ProjectRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            // File paths
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var foodFilePath = Path.Combine(baseDirectory, "data", "food.csv");
            var drinksFilePath = Path.Combine(baseDirectory, "data", "drinks.csv");
            var tablesFilePath = Path.Combine(baseDirectory, "data", "tables.csv");
            var ordersFilePath = Path.Combine(baseDirectory, "data", "orders.json");
            var restaurantCheckFilePath = Path.Combine(baseDirectory, "data", "restaurant_check.txt");
            var clientCheckBasePath = Path.Combine(baseDirectory, "data");

            // Repositories
            ITableRepository tableRepository = new TableRepository(tablesFilePath);
            IOrderRepository orderRepository = new OrderRepository(ordersFilePath);
            IItemRepository itemRepository = new ItemRepository(foodFilePath, drinksFilePath);

            // Services
            IOrderService orderService = new OrderService(orderRepository, itemRepository, tableRepository);
            ITableService tableService = new TableService(tableRepository, orderService);
            IEmailService emailService = new EmailService();
            ICheckService checkService = new CheckService(new CheckRepository());
            IRestaurantService restaurantService = new RestaurantService(tableRepository, orderRepository, orderService, checkService, emailService, restaurantCheckFilePath, clientCheckBasePath);

            // Presentation
            var menuPresentation = new MenuPresentation(restaurantService, itemRepository, orderService, tableService);

            // Run the application
            menuPresentation.DisplayMenu();
        }
    }
}
