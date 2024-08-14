using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Repositories.Interfaces;
using ProjectRestaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly ICheckService _checkService;
        private readonly IEmailService _emailService;
        private readonly string _restaurantCheckFilePath;
        private readonly string _clientCheckBasePath;

        public RestaurantService(ITableRepository tableRepository, IOrderRepository orderRepository, IOrderService orderService, ICheckService checkService, IEmailService emailService, string restaurantCheckFilePath, string clientCheckBasePath)
        {
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
            _orderService = orderService;
            _checkService = checkService;
            _emailService = emailService;
            _restaurantCheckFilePath = restaurantCheckFilePath;
            _clientCheckBasePath = clientCheckBasePath;
        }

        public void RegisterOrder()
        {
            var order = _orderService.PlaceOrder();
            if (order == null)
            {
                return;
            }
        }

        public void CompleteOrder()
        {
            var occupiedTables = GetOccupiedTables();
            if (!occupiedTables.Any())
            {
                Console.WriteLine("No occupied tables found.");
                Console.ReadKey();
                return;
            }

            int tableNumber = GetTableNumberFromUser(occupiedTables);
            if (tableNumber == -1) return;

            CompleteOrderForTable(tableNumber);
        }

        private List<Table> GetOccupiedTables()
        {
            return _tableRepository.GetAllTables().Where(t => t.IsOccupied).ToList();
        }

        private int GetTableNumberFromUser(List<Table> occupiedTables)
        {
            while (true)
            {
                Console.Clear();
                DisplayOccupiedTables(occupiedTables);

                Console.Write("Enter the table number to complete the order (or 'q' to return to the main menu): ");
                var input = Console.ReadLine();
                if (input != null && input.ToLower() == "q")
                {
                    return -1;
                }

                if (int.TryParse(input, out int tableNumber) && occupiedTables.Any(t => t.TableNumber == tableNumber))
                {
                    return tableNumber;
                }
                else
                {
                    Console.WriteLine("Invalid table number. Please try again.");
                    Console.ReadKey();
                }
            }
        }

        private void DisplayOccupiedTables(List<Table> occupiedTables)
        {
            Console.WriteLine("Occupied Tables:");
            foreach (var table in occupiedTables)
            {
                Console.WriteLine($"Table Number: {table.TableNumber}, Seats: {table.Seats}");
            }
        }

        private void CompleteOrderForTable(int tableNumber)
        {
            var order = _orderService.GetOrderForTable(tableNumber);
            if (order == null)
            {
                Console.WriteLine($"No current order found for table {tableNumber}.");
                return;
            }

            _orderService.ShowOrderForTable(tableNumber);

            var printCheck = HandleUserConfirmation("Does the client want the check to be printed? (yes/no or 'q' to return to the main menu): ");
            if (printCheck == "q") return;

            var emailCheck = HandleUserConfirmation("Does the client want to receive the check via email? (yes/no or 'q' to return to the main menu): ");
            if (emailCheck == "q") return;

            if (emailCheck == "yes" && string.IsNullOrEmpty(order.Client?.Email))
            {
                var email = GetValidEmailFromUser();
                if (email == null) return; // Return to main menu if user chooses 'q'
                if (order.Client == null) order.Client = new Client(); // Ensure Client is not null
                order.Client.Email = email;
            }

            var emailRestaurantCheck = HandleUserConfirmation("Should the restaurant check be sent via email? (yes/no or 'q' to return to the main menu): ");
            if (emailRestaurantCheck == "q") return;

            Console.Clear();

            ProcessOrder(order, printCheck == "yes", emailCheck == "yes", emailRestaurantCheck == "yes");
            _orderService.MarkOrderAsCompleted(tableNumber);
            FreeTable(tableNumber);

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private string HandleUserConfirmation(string message)
        {
            while (true)
            {
                Console.Write(message);
                var option = Console.ReadLine()?.ToLower();
                if (option == "yes" || option == "no" || option == "q")
                {
                    return option;
                }
                Console.WriteLine("Invalid input. Please enter 'yes', 'no', or 'q'.");
            }
        }

        private void ProcessOrder(Order order, bool printCheck, bool emailCheck, bool emailRestaurantCheck)
        {
            // Print and save the restaurant check
            _checkService.PrintCheck(order, isClientCheck: false);
            _checkService.SaveCheckToFile(order, _restaurantCheckFilePath, isClientCheck: false);
            Console.WriteLine("Printing restaurant check");

            if (emailRestaurantCheck)
            {
                _emailService.SendEmail("ProjectRestaurant@codeacademy.lt", "Restaurant Check", "Restaurant check details...");
            }

            // Print and save the client check if requested
            if (printCheck)
            {
                _checkService.PrintCheck(order, isClientCheck: true);
                Console.WriteLine("Printing client check");
            }

            if (emailCheck)
            {
                var clientCheckFilePath = Path.Combine(_clientCheckBasePath, $"{order.Client.Name}_check.txt");
                _checkService.SaveCheckToFile(order, clientCheckFilePath, isClientCheck: true);
                _emailService.SendEmail(order.Client.Email, "Your Order", "Order details...");
            }
        }

        private string GetValidEmailFromUser()
        {
            while (true)
            {
                Console.Write("Enter client email (or 'q' to return to the main menu): ");
                var email = Console.ReadLine() ?? string.Empty;
                if (email.ToLower() == "q")
                {
                    return null;
                }
                if (IsValidEmail(email))
                {
                    return email;
                }
                else
                {
                    Console.WriteLine("Invalid email. Please enter a valid email address.");
                }
            }
        }

        private void FreeTable(int tableNumber)
        {
            var table = _tableRepository.GetTableByNumber(tableNumber);
            if (table != null)
            {
                table.IsOccupied = false;
                _tableRepository.MarkTableAsAvailable(table.TableNumber);
            }
        }

        public List<Order> GetOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
