using System.Reflection.Metadata;
using ProjectRestaurant.Services;
using ProjectRestaurant.Models;
using ProjectRestaurant.Services.Interfaces;
using ProjectRestaurant.Repositories;

namespace ProjectRestaurant
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Restaurant system:
            //    1. Waiter should be able to register client order:
            //        1.1 Item name + price in euros
            //        1.2 Placing an order first choice should be the first table, which is occupied by the client
            //        1.3 If the table is selected it should reflect in system that the table is occupied. It should also be a possibility to select a table that is not occupied
            //        1.4 In the list the tables should be marked as occupied
            //        1.5 Food items and drinks should be separated in two files: food.csv and drinks.csv

            //    2. Order should have table information:
            //        2.1 Table number, seats at the table
            //        2.2 Ordered drinks and food. Total price to be paid
            //        2.3 Date and time of the order
                
            //    3. There should be 2 checks created from the order:
            //        3.1 One for the restaurant. One for the client. (the checks should be different)
                
            //    4. Both checks should be emailed to the client and the restaurant(Use interface for this)
                
            //    5. According to the client needs it should be possible to not print a client check, but the restaurant check should always be printed. Also restaurant check should be saved in a file.

            //    Unit tests are required for the system.

        }
    }
}
