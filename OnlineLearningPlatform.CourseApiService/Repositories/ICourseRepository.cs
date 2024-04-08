using OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;

namespace OnlineLearningPlatform.CourseApiService.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetCoursesAsync();
    }
}
