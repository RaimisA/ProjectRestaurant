using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories
{
    public class CheckRepository
    {
        public void SaveCheckToFile(Order order, string filePath, bool isClientCheck)
        {
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Order for Table {order.Table.TableNumber} at {order.OrderDateTime}: Total Price = {order.TotalPrice} EUR");
                foreach (var item in order.OrderItems)
                {
                    writer.WriteLine($"  - {item.Item.Name} x {item.Quantity} = {item.TotalPrice} EUR");
                }
                if (isClientCheck)
                {
                    writer.WriteLine("This is a client check.");
                }
                else
                {
                    writer.WriteLine("This is a restaurant check.");
                }
                writer.WriteLine();
            }
        }
    }
}
