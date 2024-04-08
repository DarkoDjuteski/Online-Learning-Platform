using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OnlineLearningPlatform.Infrastructure.Abstraction;
using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;
using System.Text;
using System.Text.Json;

namespace OnlineLearningPlatform.DocumentApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    IUnitOfWork<OnlineLearningPlatformContext> _uowGetStarted { get; set; }
    IGenericRepository<Course> _courseRepo { get; set; }

    private readonly ILogger<CourseController> _logger;

    private readonly IDistributedCache _cache;

    public CourseController(ILogger<CourseController> logger, IDistributedCache cache, IUnitOfWork<OnlineLearningPlatformContext> uowGetStarted)
    {
        _logger = logger;
        _uowGetStarted = uowGetStarted;
        _courseRepo = _uowGetStarted.GetGenericRepository<Course>();
        _cache = cache;
    }

    [HttpGet(Name = "Course")]
    public async Task<IEnumerable<Course>> Get()
    {
        var cachedCourses = await _cache.GetAsync("course");

        if (cachedCourses is null)
        {
            var courses = await _courseRepo.GetAsync();

            await _cache.SetAsync("course", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(courses.ToArray())), new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60)
            });

            return courses;
        }

        return JsonSerializer.Deserialize<IEnumerable<Course>>(cachedCourses);
    }
}
