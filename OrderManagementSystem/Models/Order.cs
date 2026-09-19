namespace OrderManagementSystem.Models
{
    public class Order
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}