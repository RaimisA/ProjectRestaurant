using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories
{
    public class CheckRepository : ICheckRepository
    {
        public void SaveCheckToFile(Order order, string filePath, bool isClientCheck)
        {
            using (var writer = new StreamWriter(filePath))
            {
                if (isClientCheck)
                { 
                    writer.WriteLine("This is a client check.");
                }
                else
                {
                    writer.WriteLine("This is a restaurant check.");
                }

                if (order.Table != null)
                {
                    writer.WriteLine($"Order for Table {order.Table.TableNumber}");
                }
                else
                {
                    writer.WriteLine("Order for an unspecified table");
                }

                foreach (var item in order.Items)
                {
                    writer.WriteLine($"{item.Item.Name} x {item.Quantity} = {item.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)} EUR");
                }
            }
        }
    }
}
