using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using System.Net;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public OrdersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            var response = await _httpClient.GetAsync(
                $"http://localhost:5044/api/inventory/{order.Product}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, new
                {
                    message = "Inventory service unavailable"
                });
            }

            var inventory =
                await response.Content.ReadFromJsonAsync<InventoryResponse>();

            if (inventory == null)
            {
                return StatusCode(500, new
                {
                    message = "Invalid inventory response"
                });
            }

            if (inventory.Available &&
                inventory.Quantity >= order.Quantity)
            {
                return Ok(new
                {
                    message = "Order placed successfully"
                });
            }

            return BadRequest(new
            {
                message = "Insufficient stock"
            });
        }
    }
}