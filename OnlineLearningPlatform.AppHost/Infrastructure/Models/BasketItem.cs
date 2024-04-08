using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.AppHost.Infrastructure.Models;

public partial class BasketItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal OldUnitPrice { get; set; }

    public int Quantity { get; set; }

    public int CustomerBasketId { get; set; }

    public virtual CustomerBasket CustomerBasket { get; set; } = null!;
}
