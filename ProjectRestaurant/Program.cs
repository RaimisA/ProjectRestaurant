using System.Reflection.Metadata;
using ProjectRestaurant.Services;
using ProjectRestaurant.Models;
using ProjectRestaurant.Services.Interfaces;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Presentation;

namespace ProjectRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Restaurant system:
            //    1. Waiter should be able to register client order:
            //        1.1 Item name + price in euros
            //        1.2 Placing an order first choice should be the first table, which is occupied by the client
            //        1.3 If the table is selected it should reflect in system that the table is occupied. It should also be a possibility to select a table that is not occupied
            //        1.4 In the list the tables should be marked as occupied
            //        1.5 Food items and drinks should be separated in two files: food.csv and drinks.csv

            //    2. Order should have table information:
            //        2.1 Table number, seats at the table
            //        2.2 Ordered drinks and food. Total price to be paid
            //        2.3 Date and time of the order

            //    3. There should be 2 checks created from the order:
            //        3.1 One for the restaurant. One for the client. (the checks should be different)

            //    4. Both checks should be emailed to the client and the restaurant(Use interface for this)

            //    5. According to the client needs it should be possible to not print a client check, but the restaurant check should always be printed. Also restaurant check should be saved in a file.

            //    Unit tests are required for the system.

            //update logic there it should be selecting tables from the list before placing an order (maybe filter tables by seats)
            //should create a new name user check if file exist
            //todo unit tests


            //file paths
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var foodFilePath = Path.Combine(baseDirectory, "data", "food.csv");
            var drinksFilePath = Path.Combine(baseDirectory, "data", "drinks.csv");
            var tablesFilePath = Path.Combine(baseDirectory, "data", "tables.csv");
            var ordersFilePath = Path.Combine(baseDirectory, "data", "orders.json");
            var restaurantCheckFilePath = Path.Combine(baseDirectory, "data", "restaurant_check.txt");
            var clientCheckBasePath = Path.Combine(baseDirectory, "data");

            var tableRepository = new TableRepository(tablesFilePath);
            var orderRepository = new OrderRepository(ordersFilePath);
            var itemRepository = new ItemRepository(foodFilePath, drinksFilePath);

            var orderService = new OrderService(orderRepository, itemRepository, tableRepository);
            var tableService = new TableService(tableRepository, orderService);
            var emailService = new EmailService();
            var checkRepository = new CheckRepository();
            var checkService = new CheckService(checkRepository);

            var restaurantService = new RestaurantService(tableRepository, orderRepository, orderService, checkService, emailService, restaurantCheckFilePath, clientCheckBasePath);

            var menuPresentation = new MenuPresentation(restaurantService, itemRepository, orderService, tableService);

            menuPresentation.DisplayMenu();
        }
    }
}
