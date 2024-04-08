using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLearningPlatform.GatewayApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly HttpClient _client;

        public OrderController(HttpClient client)
        {
            _client = client;
        }

        [HttpGet("Basket")]
        public async Task<IActionResult> GetBasket(string buyerId)
        {
            var response = await _client.GetAsync($"http://localhost:5092/Basket?buyerId={buyerId}");
            var content = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                return Content(content, "application/json");
            }
            else
            {
                return BadRequest();
            }

            //return Content(content, "application/json");
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCartAsync(string buyerId, int courseId)
        {
            var response = await _client.PostAsync($"http://localhost:5092/Basket?buyerId={buyerId}&courseId={courseId}", null);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> CheckoutBasketAsync(string buyerId)
        {
            var response = await _client.PostAsync($"http://localhost:5092/Basket/Checkout?buyerId={buyerId}", null);
            return new StatusCodeResult((int)response.StatusCode);
        }

        [HttpDelete("DeleteBasket")]
        public async Task<IActionResult> DeleteBasketAsync(string buyerId)
        {
            var response = await _client.DeleteAsync($"http://localhost:5092/Basket?buyerId={buyerId}");
            return new StatusCodeResult((int)response.StatusCode);
        }
    }
}
