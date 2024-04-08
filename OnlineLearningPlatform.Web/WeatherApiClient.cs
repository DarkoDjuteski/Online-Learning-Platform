namespace OnlineLearningPlatform.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<Course[]> GetWeatherAsync()
    {
        return await httpClient.GetFromJsonAsync<Course[]>("/course") ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

}
