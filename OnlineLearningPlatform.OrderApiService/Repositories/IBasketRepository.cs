using OnlineLearningPlatform.OrderApiService.Models;

namespace OnlineLearningPlatform.OrderApiService.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(string customerId);
        IEnumerable<string> GetUsers();
        Task<Basket?> UpdateBasketAsync(Basket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
