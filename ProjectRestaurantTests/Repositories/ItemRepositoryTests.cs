using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Repositories.Tests
{
    [TestClass]
    public class ItemRepositoryTests
    {
        private string _foodFilePath;
        private string _drinkFilePath;

        [TestInitialize]
        public void Setup()
        {
            _foodFilePath = Path.GetTempFileName();
            _drinkFilePath = Path.GetTempFileName();

            File.WriteAllLines(_foodFilePath, new string[]
            {
                "Beef Burger,10.59",
                "Steak,12.99",
                "Tuna tartar,8.50",
                "Pasta,7.25",
                "Pizza,8.99"
            });

            File.WriteAllLines(_drinkFilePath, new string[]
            {
                "Pepsi,1.99",
                "Water,0.99",
                "Beer,3.50",
                "Wine,5.00",
                "Juice,2.50",
                "Coffee,1.50",
                "Tea,1.25"
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_foodFilePath))
            {
                File.Delete(_foodFilePath);
            }

            if (File.Exists(_drinkFilePath))
            {
                File.Delete(_drinkFilePath);
            }
        }

        [TestMethod]
        public void GetFoodItems_ShouldReturnCorrectItems()
        {
            // Arrange
            var itemRepository = new ItemRepository(_foodFilePath, _drinkFilePath);

            // Act
            var foodItems = itemRepository.GetFoodItems();

            // Assert
            Assert.AreEqual(5, foodItems.Count);
            Assert.IsTrue(foodItems.Any(item => item.Name == "Beef Burger" && item.Price == 10.59m));
            Assert.IsTrue(foodItems.Any(item => item.Name == "Steak" && item.Price == 12.99m));
        }

        [TestMethod]
        public void GetDrinkItems_ShouldReturnCorrectItems()
        {
            // Arrange
            var itemRepository = new ItemRepository(_foodFilePath, _drinkFilePath);

            // Act
            var drinkItems = itemRepository.GetDrinkItems();

            // Assert
            Assert.AreEqual(7, drinkItems.Count);
            Assert.IsTrue(drinkItems.Any(item => item.Name == "Pepsi" && item.Price == 1.99m));
            Assert.IsTrue(drinkItems.Any(item => item.Name == "Water" && item.Price == 0.99m));
        }

        [TestMethod]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Arrange
            var itemRepository = new ItemRepository(_foodFilePath, _drinkFilePath);

            // Act
            var allItems = itemRepository.GetAllItems();

            // Assert
            Assert.AreEqual(12, allItems.Count);
            Assert.IsTrue(allItems.Any(item => item.Name == "Beef Burger" && item.Price == 10.59m));
            Assert.IsTrue(allItems.Any(item => item.Name == "Pepsi" && item.Price == 1.99m));
        }

        [TestMethod]
        public void EnsureFilesExist_ShouldCreateFilesIfNotFound()
        {
            // Arrange
            var nonExistentFoodFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".csv");
            var nonExistentDrinkFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".csv");

            // Act
            var itemRepository = new ItemRepository(nonExistentFoodFilePath, nonExistentDrinkFilePath);

            // Assert
            Assert.IsTrue(File.Exists(nonExistentFoodFilePath));
            Assert.IsTrue(File.Exists(nonExistentDrinkFilePath));

            // Clean up
            File.Delete(nonExistentFoodFilePath);
            File.Delete(nonExistentDrinkFilePath);
        }
    }
}