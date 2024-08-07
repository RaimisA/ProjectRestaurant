using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services
{
    public class TableService
    {
        private readonly TableRepository _tableRepository;

        public TableService(TableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public void ViewTables()
        {
            while (true)
            {
                Console.Clear();
                var tables = _tableRepository.GetAllTables();
                foreach (var table in tables)
                {
                    Console.WriteLine($"Table {table.TableNumber}: table seats: {table.Seats}, {(table.IsOccupied ? "Occupied" : "Available")}");
                }

                Console.WriteLine("Enter the table number to free up (or 'back' to return to the main menu): ");
                var input = Console.ReadLine();

                if (input != null && input.ToLower() == "back")
                {
                    break;
                }

                if (int.TryParse(input, out var tableNumber))
                {
                    var table = tables.FirstOrDefault(t => t.TableNumber == tableNumber);
                    if (table != null && table.IsOccupied)
                    {
                        _tableRepository.MarkTableAsAvailable(table.TableNumber);
                        Console.WriteLine($"Table {table.TableNumber} is now free.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Invalid table number or the table is already free.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadKey();
                }
            }
        }
    }
}
