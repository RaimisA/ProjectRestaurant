using ProjectRestaurant.Enums;

namespace ProjectRestaurant.Models
{
    public class Order
    {
        public Table? Table { get; set; }
        public Client? Client { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.TotalPrice);
            }
        }
        public Order() { }
    }
}
