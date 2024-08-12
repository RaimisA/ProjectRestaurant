using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Services.Interfaces;

namespace ProjectRestaurant.Services
{
    public class TableService : ITableService
    {
        private readonly TableRepository _tableRepository;
        private readonly OrderService _orderService;

        public TableService(TableRepository tableRepository, OrderService orderService)
        {
            _tableRepository = tableRepository;
            _orderService = orderService;
        }

        public void ViewTables()
        {
            Console.Clear();
            var tables = _tableRepository.GetAllTables();
            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found.");
                Console.ReadKey();
                return;
            }

            foreach (var table in tables)
            {
                Console.WriteLine($"Table {table.TableNumber} - Seats: {table.Seats} - {(table.IsOccupied ? "Occupied" : "Available")}");
            }

            Console.Write("Enter table number to free up (or 'q' to return to the main menu): ");
            var input = Console.ReadLine();
            if (input != null && input.ToLower() == "q")
            {
                return;
            }

            if (int.TryParse(input, out var tableNumber))
            {
                var table = tables.FirstOrDefault(t => t.TableNumber == tableNumber);
                if (table != null && table.IsOccupied)
                {
                    _tableRepository.MarkTableAsAvailable(tableNumber);
                    _orderService.MarkOrderAsCanceled(tableNumber);
                    Console.WriteLine($"Table {tableNumber} is now free and the associated order has been canceled.");
                }
                else
                {
                    Console.WriteLine("Table is either not found or already free.");
                }
            }
            else
            {
                Console.WriteLine("Invalid table number.");
            }

            Console.ReadKey();
        }
    }
}
