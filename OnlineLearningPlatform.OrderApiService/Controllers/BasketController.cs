using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.Graph;
using Microsoft.Graph.SecurityNamespace;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;
using OnlineLearningPlatform.Infrastructure.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace OnlineLearningPlatform.OrderApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IDistributedCache _cache;
    IUnitOfWork<OnlineLearningPlatformContext> _uowGetStarted { get; set; }
    private readonly IGenericRepository<CustomerBasket> _basketRepo;

    public BasketController(ILogger<BasketController> logger, IDistributedCache cache, IUnitOfWork<OnlineLearningPlatformContext> uowGetStarted)
    {
        _logger = logger;
        _cache = cache;
        _uowGetStarted = uowGetStarted;
        _basketRepo = _uowGetStarted.GetGenericRepository<CustomerBasket>();
    }

    [HttpGet(Name = "Basket")]
    public async Task<CustomerBasket> GetBasket(string buyerId)
    {
        //var cachedBasket = await _cache.GetAsync("basket");

        //if (cachedBasket is null)
        //{
        var baskets = _basketRepo.GetAsQueryable(b => b.BuyerId == buyerId).Include(x=>x.Items).FirstOrDefault();

        //await _cache.SetAsync("basket", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(baskets.ToArray())), new()
        //{
        //    AbsoluteExpiration = DateTime.Now.AddSeconds(60)
        //});

        return baskets;
        //}

        //return JsonSerializer.Deserialize<IEnumerable<CustomerBasket>>(cachedBasket);
    }

    [HttpPost]
    public async Task<CustomerBasket> AddToCartAsync(string buyerId, int courseId)
    {
        var basket = _basketRepo.GetAsQueryable(b => b.BuyerId == buyerId).Include(x=>x.Items).FirstOrDefault();
        var found = false;

        if (basket == null)
        {
            basket = new CustomerBasket { BuyerId = buyerId, Items = new List<BasketItem>() };
        }
        foreach (var item in basket.Items)
        {
            if (item.ProductId == courseId)
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
                ProductId = courseId
            });
        }

        await _basketRepo.UpsertAsync(basket, x => x.BuyerId == basket.BuyerId);
        await _uowGetStarted.SaveChangesAsync();
        return basket;
    }

    [HttpPost("Checkout")]
    public async Task CheckoutBasketAsync(string buyerId)
    {
        var basket = _basketRepo.GetAsQueryable(b => b.BuyerId == buyerId).FirstOrDefault();
        //basket.CheckedOut = true;
        await _basketRepo.UpsertAsync(basket, x => x.BuyerId == basket.BuyerId);
        await _uowGetStarted.SaveChangesAsync();
    }

    [HttpDelete]
    public async Task DeleteBasketAsync(string buyerId)
    {
        var basket = _basketRepo.GetAsQueryable(b => b.BuyerId == buyerId).FirstOrDefault();
        await _basketRepo.DeleteAsync(int.Parse(basket.BuyerId));
        await _uowGetStarted.SaveChangesAsync();
    }
}
