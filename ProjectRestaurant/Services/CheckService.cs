using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories;
using ProjectRestaurant.Repositories.Interfaces;
using ProjectRestaurant.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace ProjectRestaurant.Services
{
    public class CheckService : ICheckService
    {
        private readonly ICheckRepository _checkRepository;
        private readonly CultureInfo _cultureInfo;

        public CheckService(ICheckRepository checkRepository)
        {
            _checkRepository = checkRepository;
            _cultureInfo = new CultureInfo("fr-FR"); // change to show eur symbol correctly
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
            foreach (var orderItem in order.Items)
            {
                Console.WriteLine($"{orderItem.Item.Name.PadRight(20)} x{orderItem.Quantity} - {orderItem.TotalPrice.ToString("C", _cultureInfo)}");
            }
        }

        private void PrintClientFooter(Order order)
        {
            Console.WriteLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");
            Console.WriteLine($"Date: {order.DateTime.ToString("g", _cultureInfo)}");
            Console.WriteLine("Thank you for dining with us!");
            Console.WriteLine("Contact us at: ProjectRestaurant@codeacademy.lt");
        }

        private void PrintRestaurantFooter(Order order)
        {
            Console.WriteLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");
            Console.WriteLine($"Date: {order.DateTime.ToString("g", _cultureInfo)}");
            Console.WriteLine($"Table: {order.Table.TableNumber}");
            Console.WriteLine("Internal Notes for restaurant: Ensure inventory is updated.");
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

            foreach (var orderItem in order.Items)
            {
                checkContent.AppendLine($"{orderItem.Item.Name.PadRight(20)} x{orderItem.Quantity} - {orderItem.TotalPrice.ToString("C", _cultureInfo)}");
            }

            checkContent.AppendLine($"Total: {order.TotalPrice.ToString("C", _cultureInfo)}");

            if (isClientCheck)
            {
                checkContent.AppendLine($"Date: {order.DateTime.ToString("g", _cultureInfo)}");
                checkContent.AppendLine("Thank you for dining with us!");
                checkContent.AppendLine("Contact us at: ProjectRestaurant@codeacademy.lt");
            }
            else
            {
                checkContent.AppendLine($"Date: {order.DateTime.ToString("g", _cultureInfo)}");
                checkContent.AppendLine($"Table: {order.Table.TableNumber}");
                checkContent.AppendLine("Internal Notes for restaurant: Ensure inventory is updated.");
            }

            File.WriteAllText(filePath, checkContent.ToString());
        }
    }
}
