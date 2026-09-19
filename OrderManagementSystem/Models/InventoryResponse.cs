namespace OrderManagementSystem.Models
{
    public class InventoryResponse
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool Available { get; set; }
    }
}