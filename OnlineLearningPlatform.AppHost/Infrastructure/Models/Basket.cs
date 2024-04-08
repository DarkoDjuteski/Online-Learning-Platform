using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.AppHost.Infrastructure.Models;

public partial class Basket
{
    public int BasketId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public int Quantity { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
