namespace ProjectRestaurant.Models
{
    public class Order
    {
        public Table Table { get; set; }
        public Client Client { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime OrderDateTime { get; set; } = DateTime.Now;

        public decimal TotalPrice => OrderItems.Sum(item => item.TotalPrice);

        public Order() { }

        public Order(Table table, DateTime orderDateTime, Client client, List<OrderItem> orderItems)
        {
            Table = table;
            OrderDateTime = orderDateTime;
            Client = client;
            OrderItems = orderItems;
        }

        public Order(Table table, DateTime orderDateTime, Client client, decimal totalPrice)
        {
            Table = table;
            OrderDateTime = orderDateTime;
            Client = client;
            SetTotalPrice(totalPrice);
        }

        private void SetTotalPrice(decimal totalPrice)
        {
            // Method used to set the TotalPrice value
            // Used in the repository when the order is registered from a file
            foreach (var item in OrderItems)
            {
                totalPrice -= item.TotalPrice;
            }
        }
    }
}
