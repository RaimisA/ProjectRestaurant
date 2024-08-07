using ProjectRestaurant.Models;
using System.Text;

namespace ProjectRestaurant.Services
{
    public class CheckService
    {
        public void PrintCheck(Order order, bool isClientCheck)
        {
            var checkDetails = new StringBuilder();
            checkDetails.AppendLine("Order Details:");
            checkDetails.AppendLine($"Table Number: {order.Table.TableNumber}");
            checkDetails.AppendLine($"Client: {order.Client.Name} ({order.Client.Email})");
            checkDetails.AppendLine("Items:");

            foreach (var orderItem in order.OrderItems)
            {
                checkDetails.AppendLine($"- {orderItem.Item.Name}: {orderItem.Item.Price} EUR x {orderItem.Quantity} = {orderItem.TotalPrice} EUR");
            }

            checkDetails.AppendLine($"Total Price: {order.TotalPrice} EUR");

            if (isClientCheck)
            {
                Console.WriteLine(checkDetails.ToString());
            }
        }

        public void SaveCheckToFile(Order order, string filePath)
        {
            var checkDetails = new StringBuilder();
            checkDetails.AppendLine("Order Details:");
            checkDetails.AppendLine($"Table Number: {order.Table.TableNumber}");
            checkDetails.AppendLine($"Client: {order.Client.Name} ({order.Client.Email})");
            checkDetails.AppendLine("Items:");

            foreach (var orderItem in order.OrderItems)
            {
                checkDetails.AppendLine($"- {orderItem.Item.Name}: {orderItem.Item.Price} EUR x {orderItem.Quantity} = {orderItem.TotalPrice} EUR");
            }

            checkDetails.AppendLine($"Total Price: {order.TotalPrice} EUR");

            File.WriteAllText(filePath, checkDetails.ToString());
        }
    }
}
