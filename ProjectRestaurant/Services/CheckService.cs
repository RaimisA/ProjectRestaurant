using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace ProjectRestaurant.Services
{
    public class CheckService : ICheckService
    {
        private readonly CheckRepository _checkRepository;
        private readonly CultureInfo _cultureInfo;

        public CheckService(CheckRepository checkRepository)
        {
            _checkRepository = checkRepository;
            _cultureInfo = new CultureInfo("en-US"); // change to show eur symbol correctly
        }

        public void PrintCheck(Order order, bool isClientCheck)
        {
            if (isClientCheck)
            {
                PrintClientCheck(order);
            }
            else
            {
                PrintRestaurantCheck(order);
            }
        }

        private void PrintClientCheck(Order order)
        {
            PrintHeader("Client Check");
            PrintOrderDetails(order);
            PrintClientFooter(order);
        }

        private void PrintRestaurantCheck(Order order)
        {
            PrintHeader("Restaurant Check");
            PrintOrderDetails(order);
            PrintRestaurantFooter(order);
        }

        private void PrintHeader(string checkType)
        {
            Console.WriteLine("╔═════════════════════════╗");
            Console.WriteLine($"║       {checkType.PadRight(17)} ║");
            Console.WriteLine("╚═════════════════════════╝");
        }

        private void PrintOrderDetails(Order order)
        {
            Console.WriteLine("╔═════════════════════════╗");
            Console.WriteLine("║       Order Details     ║");
            Console.WriteLine("╚═════════════════════════╝");
            foreach (var orderItem in order.OrderItems)
            {
                Console.WriteLine($"{orderItem.Item.Name.PadRight(20)} x{orderItem.Quantity} - {orderItem.TotalPrice.ToString("C", _cultureInfo)}");
            }
        }

        private void PrintClientFooter(Order order)
        {
            Console.WriteLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");
            Console.WriteLine("Thank you for dining with us!");
            Console.WriteLine("Contact us at: ProjectRestaurant@codeacademy.lt");
        }

        private void PrintRestaurantFooter(Order order)
        {
            Console.WriteLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");
            Console.WriteLine($"Table: {order.Table.TableNumber}");
            Console.WriteLine("Internal Notes: Ensure inventory is updated.");
        }

        public void SaveCheckToFile(Order order, string filePath, bool isClientCheck)
        {
            var checkContent = new StringBuilder();

            if (isClientCheck)
            {
                checkContent.AppendLine("╔═════════════════════════╗");
                checkContent.AppendLine("║       Client Check      ║");
                checkContent.AppendLine("╚═════════════════════════╝");
            }
            else
            {
                checkContent.AppendLine("╔═════════════════════════╗");
                checkContent.AppendLine("║     Restaurant Check    ║");
                checkContent.AppendLine("╚═════════════════════════╝");
            }

            checkContent.AppendLine("╔═════════════════════════╗");
            checkContent.AppendLine("║       Order Details     ║");
            checkContent.AppendLine("╚═════════════════════════╝");

            foreach (var orderItem in order.OrderItems)
            {
                checkContent.AppendLine($"{orderItem.Item.Name.PadRight(20)} x{orderItem.Quantity} - {orderItem.TotalPrice.ToString("C", _cultureInfo)}");
            }

            checkContent.AppendLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");

            if (isClientCheck)
            {
                checkContent.AppendLine("Thank you for dining with us!");
                checkContent.AppendLine("Contact us at: ProjectRestaurant@codeacademy.lt");
            }
            else
            {
                checkContent.AppendLine($"Table: {order.Table.TableNumber}");
                checkContent.AppendLine("Internal Notes: Ensure inventory is updated.");
            }

            File.WriteAllText(filePath, checkContent.ToString());
        }
    }
}
