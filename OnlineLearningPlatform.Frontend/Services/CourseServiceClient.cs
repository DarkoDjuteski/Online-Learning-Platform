using System.Globalization;

namespace OnlineLearningPlatform.Frontend.Services;

public class CourseServiceClient(HttpClient client)
{
    public Task<Course[]> GetItemsAsync(int? before = null, int? after = null)
    {
        // Make the query string with encoded parameters
        var query = (before, after) switch
        {
            (null, null) => default,
            (int b, null) => QueryString.Create("before", b.ToString(CultureInfo.InvariantCulture)),
            (null, int a) => QueryString.Create("after", a.ToString(CultureInfo.InvariantCulture)),
            _ => throw new InvalidOperationException(),
        };

        var result  = client.GetFromJsonAsync<Course[]>("/course");
        return result;
    }
}

public record CoursePage(int FirstId, int NextId, bool IsLastPage, IEnumerable<Course> Data);

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

}