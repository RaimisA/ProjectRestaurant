using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Models;
using ProjectRestaurant.Repositories.Interfaces;
using ProjectRestaurant.Services;
using ProjectRestaurant.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Tests
{
    public class TestTableRepository : ITableRepository
    {
        private List<Table> _tables = new List<Table>();

        public List<Table> GetAllTables()
        {
            return _tables;
        }

        public void MarkTableAsAvailable(int tableNumber)
        {
            var table = _tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            if (table != null)
            {
                table.IsOccupied = false;
            }
        }

        public void AddTable(Table table)
        {
            _tables.Add(table);
        }

        public Table? GetTableByNumber(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public void MarkTableAsOccupied(int tableNumber)
        {
            throw new NotImplementedException();
        }
    }

    public class TestOrderService : IOrderService
    {
        public Order? GetOrderForTable(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public void MarkOrderAsCanceled(int tableNumber)
        {
            // Simulate marking an order as canceled
        }

        public void MarkOrderAsCompleted(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Order? PlaceOrder()
        {
            throw new NotImplementedException();
        }

        public void ShowOrderForTable(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public void ViewOrders()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TableServiceTests
    {
        private TestTableRepository _testTableRepository;
        private TestOrderService _testOrderService;
        private TableService _tableService;

        [TestInitialize]
        public void Setup()
        {
            _testTableRepository = new TestTableRepository();
            _testOrderService = new TestOrderService();
            _tableService = new TableService(_testTableRepository, _testOrderService);
        }

        [TestMethod]
        public void TableServiceTest()
        {
            // Arrange
            // No specific arrangement needed for constructor test

            // Act
            var service = new TableService(_testTableRepository, _testOrderService);

            // Assert
            Assert.IsNotNull(service);
        }
    }
}   