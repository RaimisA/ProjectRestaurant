namespace ProjectRestaurant.Models
{
    public class Order
    {
        public Table Table { get; set; }
        public Client Client { get; set; }
        public List<Item> FoodItems { get; set; } = new List<Item>();
        public List<Item> DrinkItems { get; set; } = new List<Item>();
        public DateTime OrderDateTime { get; set; } = DateTime.Now;

        public decimal TotalPrice => CalculateTotalPrice();

        private decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var item in FoodItems)
            {
                total += item.Price;
            }
            foreach (var item in DrinkItems)
            {
                total += item.Price;
            }
            return total;
        }
    }
}
