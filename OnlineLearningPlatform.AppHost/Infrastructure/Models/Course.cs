using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.AppHost.Infrastructure.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
}
