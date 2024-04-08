using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
