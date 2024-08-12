namespace ProjectRestaurant.Models
{
    public class Order
    {
        //public int Id { get; set; }
        public Table? Table { get; set; }
        public Client? Client { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public bool IsInProgress { get; set; } = true;
        public bool IsCompleted { get; set; } = false;
        public bool IsCanceled { get; set; } = false;

        public decimal TotalPrice => OrderItems.Sum(item => item.TotalPrice);

        public Order() { }

        //public Order(Table table, DateTime orderDateTime, Client client, List<OrderItem> orderItems)
        //{
        //    Table = table;
        //    OrderDateTime = orderDateTime;
        //    Client = client;
        //    OrderItems = orderItems;
        //}

        //public Order(Table table, DateTime orderDateTime, Client client)
        //{
        //    Table = table;
        //    OrderDateTime = orderDateTime;
        //    Client = client;
        //}
    }
}
