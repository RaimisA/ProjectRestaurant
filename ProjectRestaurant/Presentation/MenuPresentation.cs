using ProjectRestaurant.Models;
using ProjectRestaurant.Services;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Presentation
{
    public class MenuPresentation
    {
        private readonly RestaurantService _restaurantService;
        private readonly ItemRepository _itemRepository;
        private readonly OrderService _orderService;
        private readonly TableService _tableService;

        public MenuPresentation(RestaurantService restaurantService, ItemRepository itemRepository, OrderService orderService, TableService tableService)
        {
            _restaurantService = restaurantService;
            _itemRepository = itemRepository;
            _orderService = orderService;
            _tableService = tableService;
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Place Order");
                Console.WriteLine("2. View Tables");
                Console.WriteLine("3. View Orders");
                Console.WriteLine("4. Complete Order");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _restaurantService.RegisterOrder();
                        break;
                    case "2":
                        _tableService.ViewTables();
                        break;
                    case "3":
                        _orderService.ViewOrders();
                        break;
                    case "4":
                        _restaurantService.CompleteOrder();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}

