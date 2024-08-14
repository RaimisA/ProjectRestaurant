using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories.Interfaces;

namespace ProjectRestaurant.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly string _filePath;
        private List<Table> tables = new List<Table>();

        public TableRepository(string filePath)
        {
            _filePath = filePath;
            EnsureFileExists();
            LoadTablesFromFile();
        }

        public List<Table> GetAllTables()
        {
            return tables;
        }

        public Table? GetTableByNumber(int tableNumber)
        {
            return tables.FirstOrDefault(t => t.TableNumber == tableNumber);
        }

        public void MarkTableAsOccupied(int tableNumber)
        {
            var table = GetTableByNumber(tableNumber);
            if (table != null)
            {
                table.IsOccupied = true;
                SaveTablesToFile();
            }
        }

        public void MarkTableAsAvailable(int tableNumber)
        {
            var table = GetTableByNumber(tableNumber);
            if (table != null)
            {
                table.IsOccupied = false;
                SaveTablesToFile();
            }
        }

        private void LoadTablesFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3 && int.TryParse(parts[0], out var tableNumber) && int.TryParse(parts[1], out var seats) && bool.TryParse(parts[2], out var isOccupied))
                {
                    tables.Add(new Table
                    {
                        TableNumber = tableNumber,
                        Seats = seats,
                        IsOccupied = isOccupied
                    });
                }
            }
        }

        private void SaveTablesToFile()
        {
            var lines = tables.Select(t => $"{t.TableNumber},{t.Seats},{t.IsOccupied}");
            File.WriteAllLines(_filePath, lines);
        }

        private void EnsureFileExists()
        {
            CreateFileIfNotFound(_filePath, new string[]
            {
                "1,4,False",
                "2,4,False",
                "3,2,False",
                "4,6,False"
            });
        }

        private void CreateFileIfNotFound(string filePath, string[] content)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllLines(filePath, content);
                Console.WriteLine($"{filePath} created.");
            }
            else
            {
                Console.WriteLine($"{filePath} already exists.");
            }
        }
    }
}
