using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private static readonly List<Product> Products = new()
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Laptop",
                Quantity = 15
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Phone",
                Quantity = 10
            }
        };

        [HttpGet("{productName}")]
        public IActionResult GetInventory(string productName)
        {
            var product = Products.FirstOrDefault(p =>
                p.ProductName.Equals(
                    productName,
                    StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                productName = product.ProductName,
                quantity = product.Quantity,
                available = product.Quantity > 0
            });
        }
    }
}