namespace OnlineLearningPlatform.OrderApiService.Models
{
    public partial class Basket
    {
        public int BasketId { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public int Quantity { get; set; }

    }
}
