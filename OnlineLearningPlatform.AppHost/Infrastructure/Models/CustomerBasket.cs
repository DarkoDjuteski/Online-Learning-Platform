using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.AppHost.Infrastructure.Models;

public partial class CustomerBasket
{
    public int Id { get; set; }

    public string BuyerId { get; set; } = null!;

    public int TotalItemCount { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
