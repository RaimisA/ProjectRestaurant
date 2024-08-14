using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Services.Interfaces;
using ProjectRestaurant.Enums;
using ProjectRestaurant.Repositories.Interfaces;

namespace ProjectRestaurant.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ITableRepository _tableRepository;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, ITableRepository tableRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _tableRepository = tableRepository;
        }

        public Order? PlaceOrder()
        {
            Console.Clear();
            int numberOfPeople;
            Table? table;

            while (true)
            {
                numberOfPeople = GetNumberOfPeople();
                if (numberOfPeople == -1) return null;

                table = GetAvailableTable(numberOfPeople);
                if (table != null) break;

                Console.WriteLine("No available tables with enough seats. Please try again.");
                Console.ReadKey();
            }

            var client = GetClientDetails();
            if (client == null) return null;

            var orderItems = GetOrderItems();
            if (orderItems == null) return null;

            var order = CreateOrder(table, client, orderItems);
            SaveOrder(order);

            return order;
        }

        public void ViewOrders()
        {
            Console.Clear();
            var orders = _orderRepository.GetAllOrders();
            if (!orders.Any())
            {
                Console.WriteLine("No orders found.");
                Console.WriteLine("Press any key to return to Main menu");
                Console.ReadKey();
                return;
            }

            DisplayOrders(orders);
            Console.WriteLine("Press any key to return to Main menu");
            Console.ReadKey();
        }

        public Order? GetOrderForTable(int tableNumber)
        {
            return _orderRepository.GetAllOrders()
                .FirstOrDefault(o => o.Table?.TableNumber == tableNumber && o.Status == OrderStatus.InProgress);
        }

        public void ShowOrderForTable(int tableNumber)
        {
            var order = GetOrderForTable(tableNumber);
            if (order == null)
            {
                Console.WriteLine($"No current order found for table {tableNumber}.");
                return;
            }

            DisplayOrder(order);
        }

        public void MarkOrderAsCompleted(int tableNumber)
        {
            var order = GetOrderForTable(tableNumber);
            if (order != null)
            {
                _orderRepository.MarkOrderComplete(tableNumber);
            }
        }

        public void MarkOrderAsCanceled(int tableNumber)
        {
            _orderRepository.MarkOrderCanceled(tableNumber);
        }

        private int GetNumberOfPeople()
        {
            while (true)
            {
                Console.Write("Enter the number of people (or 'q' to return to the main menu): ");
                var input = Console.ReadLine();
                if (input != null && input.ToLower() == "q")
                {
                    return -1;
                }
                if (int.TryParse(input, out int numberOfPeople) && numberOfPeople > 0)
                {
                    return numberOfPeople;
                }
                Console.WriteLine("Invalid number of people. Please enter a positive integer.");
            }
        }

        private Table? GetAvailableTable(int numberOfPeople)
        {
            var table = _tableRepository.GetAllTables().FirstOrDefault(t => !t.IsOccupied && t.Seats >= numberOfPeople);
            return table;
        }

        private Client? GetClientDetails()
        {
            Console.Write("Enter client name (or 'q' to return to the main menu): ");
            var clientName = Console.ReadLine();
            if (clientName != null && clientName.ToLower() == "q")
            {
                return null;
            }
            if (string.IsNullOrEmpty(clientName))
            {
                Console.WriteLine("Client name cannot be empty.");
                return null;
            }

            return new Client { Name = clientName };
        }

        private List<OrderItem>? GetOrderItems()
        {
            var orderItems = new List<OrderItem>();

            while (true)
            {
                Console.Clear();
                DisplayAvailableItems();

                Console.Write("Enter item name to add to order (or 'done' to finish): ");
                var itemName = Console.ReadLine();
                if (itemName != null && itemName.ToLower() == "q")
                {
                    return null;
                }
                if (string.IsNullOrEmpty(itemName))
                {
                    Console.WriteLine("Item name cannot be empty.");
                    continue;
                }

                if (itemName.ToLower() == "done")
                {
                    break;
                }

                var selectedItem = GetSelectedItem(itemName);
                if (selectedItem != null)
                {
                    var quantity = GetItemQuantity();
                    if (quantity == -1) return null;

                    orderItems.Add(new OrderItem { Item = selectedItem, Quantity = quantity });
                }
                else
                {
                    Console.WriteLine("Item not found. Please try again.");
                }
            }
            return orderItems;
        }

        private void DisplayAvailableItems()
        {
            Console.WriteLine("Available items:");
            var availableItems = _itemRepository.GetAllItems();
            foreach (var item in availableItems)
            {
                Console.WriteLine($"{item.Name} - {item.Price} EUR");
            }
        }

        private Item? GetSelectedItem(string itemName)
        {
            var availableItems = _itemRepository.GetAllItems();
            return availableItems.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        private int GetItemQuantity()
        {
            while (true)
            {
                Console.Write("Enter quantity (or 'q' to return to the main menu): ");
                var quantityInput = Console.ReadLine();
                if (quantityInput != null && quantityInput.ToLower() == "q")
                {
                    return -1;
                }
                if (int.TryParse(quantityInput, out var quantity) && quantity > 0)
                {
                    return quantity;
                }
                Console.WriteLine("Invalid quantity. Please try again.");
            }
        }

        private Order CreateOrder(Table table, Client client, List<OrderItem> orderItems)
        {
            return new Order
            {
                Table = table,
                Client = client,
                Items = orderItems,
                DateTime = DateTime.Now
            };
        }

        private void SaveOrder(Order order)
        {
            _orderRepository.AddOrder(order);
            _tableRepository.MarkTableAsOccupied(order.Table.TableNumber);
            _orderRepository.MarkOrderInProgress(order.Table.TableNumber);
            Console.WriteLine("Order registered successfully. Press any key: ");
            Console.ReadKey();
        }

        private void DisplayOrders(List<Order> orders)
        {
            foreach (var order in orders)
            {
                string status = GetOrderStatus(order);
                Console.WriteLine($"Order for Table {order.Table?.TableNumber} at {order.DateTime}: Total Price = {order.TotalPrice} EUR {status}");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
                }
            }
        }

        private string GetOrderStatus(Order order)
        {
            if (order.Status == OrderStatus.InProgress)
            {
                return "(Order in progress)";
            }
            if (order.Status == OrderStatus.Completed)
            {
                return "(Order completed)";
            }
            if (order.Status == OrderStatus.Canceled)
            {
                return "(Order canceled)";
            }
            if (order.Status == OrderStatus.New)
            {
                return "(New order)";
            }

            else return "Unknown Order";
        }

        private void DisplayOrder(Order order)
        {
            Console.WriteLine($"Order for Table {order.Table?.TableNumber} at {order.DateTime}: Total Price = {order.TotalPrice} EUR");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
            }
        }
    }
}
