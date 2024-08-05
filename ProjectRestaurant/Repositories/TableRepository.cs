using ProjectRestaurant.Models;

namespace ProjectRestaurant.Repositories
{
    public class TableRepository
    {
        private List<Table> tables = new List<Table>();

        public List<Table> GetAllTables()
        {
            return tables;
        }

        public void AddTable(Table table)
        {
            tables.Add(table);
        }

        public void MarkTableAsOccupied(int tableNumber)
        {
            var table = tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            if (table != null)
            {
                table.IsOccupied = true;
            }
        }
    }
}
