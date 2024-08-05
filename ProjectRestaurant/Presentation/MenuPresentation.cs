using ProjectRestaurant.Models;
using ProjectRestaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Presentation
{
    public class MenuPresentation
    {
        private readonly Restaurant _restaurant;

        public MenuPresentation(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Register Order");
                Console.WriteLine("2. View Tables");
                Console.WriteLine("3. View Orders");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterOrder();
                        break;
                    case "2":
                        ViewTables();
                        break;
                    case "3":
                        ViewOrders();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void RegisterOrder()
        {
            // Example implementation for registering an order
            var table = _restaurant.GetTables().FirstOrDefault(t => !t.IsOccupied);
            if (table == null)
            {
                Console.WriteLine("No available tables.");
                return;
            }

            Console.Write("Enter client name: ");
            var clientName = Console.ReadLine();
            Console.Write("Enter client email: ");
            var clientEmail = Console.ReadLine();

            var client = new Client
            {
                Name = clientName,
                Email = clientEmail
            };

            var order = new Order
            {
                Table = table,
                Client = client,
                FoodItems = new List<Item> { new Item { Name = "Pizza", Price = 10.0m } },
                DrinkItems = new List<Item> { new Item { Name = "Coke", Price = 2.0m } }
            };

            _restaurant.RegisterOrder(order);
            Console.WriteLine("Order registered successfully.");
        }

        private void ViewTables()
        {
            var tables = _restaurant.GetTables();
            foreach (var table in tables)
            {
                Console.WriteLine($"Table {table.TableNumber}: {(table.IsOccupied ? "Occupied" : "Available")}");
            }
        }

        private void ViewOrders()
        {
            var orders = _restaurant.GetOrders();
            foreach (var order in orders)
            {
                Console.WriteLine($"Order for Table {order.Table.TableNumber} at {order.OrderDateTime}: Total Price = {order.TotalPrice} EUR");
            }
        }
    }
}

