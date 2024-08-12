using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Services.Interfaces;

namespace ProjectRestaurant.Services
{
    public class OrderService : IOrderService
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

        public Order? PlaceOrder()
        {
            Console.Clear();
            int numberOfPeople = GetNumberOfPeople();
            if (numberOfPeople == -1) return null;

            var table = GetAvailableTable(numberOfPeople);
            if (table == null) return null;

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
                .FirstOrDefault(o => o.Table?.TableNumber == tableNumber && o.IsInProgress);
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
            if (table == null)
            {
                Console.WriteLine("No available tables with enough seats.");
            }
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
                OrderItems = orderItems,
                OrderDateTime = DateTime.Now
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
                Console.WriteLine($"Order for Table {order.Table?.TableNumber} at {order.OrderDateTime}: Total Price = {order.TotalPrice} EUR {status}");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
                }
            }
        }

        private string GetOrderStatus(Order order)
        {
            if (order.IsCompleted)
            {
                return "(Order Completed)";
            }
            if (order.IsCanceled)
            {
                return "(Order Canceled)";
            }
            if (order.IsInProgress)
            {
                return "(Order in progress)";
            }
            return "(Unknown Status)";
        }

        private void DisplayOrder(Order order)
        {
            Console.WriteLine($"Order for Table {order.Table?.TableNumber} at {order.OrderDateTime}: Total Price = {order.TotalPrice} EUR");
            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
            }
        }
    }
}
