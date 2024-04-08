using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;

public partial class CustomerBasket
{
    public int Id { get; set; }

    public string BuyerId { get; set; } = null!;

    public int TotalItemCount => Items.Sum(i => i.Quantity);

    public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
}
