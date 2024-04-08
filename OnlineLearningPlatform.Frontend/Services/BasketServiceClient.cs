using Grpc.Core;
using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;
using Polly.Timeout;
using System.Text;
using System.Text.Json;
namespace OnlineLearningPlatform.Frontend.Services;

public class BasketServiceClient(HttpClient client)
{
    public async Task<(CustomerBasket? CustomerBasket, bool IsAvailable)> GetBasketAsync(string buyerId)
    {
        try
        {
            var response = await client.GetAsync($"/order/basket?buyerId={buyerId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var basket = new CustomerBasket();
                if (!string.IsNullOrEmpty(content))
                {
                    basket = JsonSerializer.Deserialize<CustomerBasket>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return (basket, true);
                }
                else
                {
                    return (null, true);
                }
                
            }
            return (null, false);
        }
        catch (RpcException ex) when (
            // Service name could not be resolved
            ex.StatusCode is StatusCode.Unavailable ||
            // Polly resilience timed out after retries
            (ex.StatusCode is StatusCode.Internal && ex.Status.DebugException is TimeoutRejectedException))
        {
            return (null, false);
        }
    }

    public async Task<CustomerBasket> AddToCartAsync(string buyerId, int productId)
    {
        var (basket, _) = await GetBasketAsync(buyerId);
        if(basket is null)
        {
            basket = new CustomerBasket() { BuyerId = buyerId };
        }
        var found = false;
        foreach (var item in basket.Items)
        {
            if (item.ProductId == productId)
            {
                ++item.Quantity;
                found = true;
                break;
            }
        }

        if (!found)
        {
            basket.Items.Add(new BasketItem
            {
                //Id = Guid.NewGuid().ToString("N"),
                Quantity = 1,
                ProductId = productId
            });
        }

        var json = JsonSerializer.Serialize(basket);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"/order/AddToCart?buyerId={buyerId}&courseId={productId}", null);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerBasket>(responseContent);

    }

    public async Task CheckoutBasketAsync(string buyerId)
    {
        await client.PostAsync($"/order/Checkout?buyerId={buyerId}", null);
    }

    public async Task DeleteBasketAsync(string buyerId)
    {
        await client.DeleteAsync($"/order/DeleteBasket?buyerId={buyerId}");
    }
}
