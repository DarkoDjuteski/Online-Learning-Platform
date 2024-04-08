using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;
using StackExchange.Redis;
using System.Text.Json;

namespace OnlineLearningPlatform.CourseApiService.Repositories
{

    public class RedisCourseRepository(ILogger<RedisCourseRepository> logger, IConnectionMultiplexer redis) : ICourseRepository
    {
        private readonly ILogger<RedisCourseRepository> _logger = logger;
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly IDatabase _database = redis.GetDatabase();
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Course?> GetCoursesAsync()
        {
            var data = await _database.StringGetAsync("course");

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<Course>(data!, _jsonSerializerOptions);
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
