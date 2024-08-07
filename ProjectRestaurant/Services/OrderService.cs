using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ItemRepository _itemRepository;
        private readonly TableRepository _tableRepository;

        public OrderService(OrderRepository orderRepository, ItemRepository itemRepository, TableRepository tableRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _tableRepository = tableRepository;
        }

        public void PlaceOrder()
        {
            Console.Clear();
            Console.Write("Enter the number of people: ");
            if (!int.TryParse(Console.ReadLine(), out var numberOfPeople) || numberOfPeople <= 0)
            {
                Console.WriteLine("Invalid number of people.");
                Console.ReadKey();
                return;
            }

            var table = _tableRepository.GetAllTables().FirstOrDefault(t => !t.IsOccupied && t.Seats >= numberOfPeople);
            if (table == null)
            {
                Console.WriteLine("No available tables with enough seats.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter client name: ");
            var clientName = Console.ReadLine();
            if (string.IsNullOrEmpty(clientName))
            {
                Console.WriteLine("Client name cannot be empty.");
                Console.ReadKey();
                return;
            }

            Console.Write("Does the client want to receive an email? (yes/no): ");
            var emailOption = Console.ReadLine();
            string clientEmail = string.Empty;

            if (emailOption != null && emailOption.ToLower() == "yes")
            {
                Console.Write("Enter client email: ");
                clientEmail = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(clientEmail))
                {
                    Console.WriteLine("Client email cannot be empty.");
                    Console.ReadKey();
                    return;
                }
            }

            var client = new Client
            {
                Name = clientName,
                Email = clientEmail
            };

            var orderItems = GetOrderItems();

            var order = new Order
            {
                Table = table,
                Client = client,
                OrderItems = orderItems,
                OrderDateTime = DateTime.Now
            };

            _orderRepository.AddOrder(order);
            _tableRepository.MarkTableAsOccupied(table.TableNumber);
            Console.WriteLine("Order registered successfully.");
            Console.ReadKey();
        }

        public void ViewOrders()
        {
            Console.Clear();
            var orders = _orderRepository.GetAllOrders();
            foreach (var order in orders)
            {
                Console.WriteLine($"Order for Table {order.Table.TableNumber} at {order.OrderDateTime}: Total Price = {order.TotalPrice} EUR");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
                }
            }
            Console.ReadKey();
        }

        private List<OrderItem> GetOrderItems()
        {
            var orderItems = new List<OrderItem>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Available items:");
                var availableItems = _itemRepository.GetAllItems();
                foreach (var item in availableItems)
                {
                    Console.WriteLine($"{item.Name} - {item.Price} EUR");
                }

                Console.Write("Enter item name to add to order (or 'done' to finish): ");
                var itemName = Console.ReadLine();
                if (string.IsNullOrEmpty(itemName))
                {
                    Console.WriteLine("Item name cannot be empty.");
                    continue;
                }

                if (itemName.ToLower() == "done")
                {
                    break;
                }

                var selectedItem = availableItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (selectedItem != null)
                {
                    Console.Write("Enter quantity: ");
                    if (int.TryParse(Console.ReadLine(), out var quantity) && quantity > 0)
                    {
                        orderItems.Add(new OrderItem { Item = selectedItem, Quantity = quantity });
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Item not found. Please try again.");
                }
            }
            return orderItems;
        }
    }
}
