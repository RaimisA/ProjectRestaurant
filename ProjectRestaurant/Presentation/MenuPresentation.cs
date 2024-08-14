using ProjectRestaurant.Models;
using ProjectRestaurant.Services;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Services.Interfaces;

namespace ProjectRestaurant.Presentation
{
    public class MenuPresentation
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IItemRepository _itemRepository;
        private readonly IOrderService _orderService;
        private readonly ITableService _tableService;

        public MenuPresentation(IRestaurantService restaurantService, IItemRepository itemRepository, IOrderService orderService, ITableService tableService)
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

