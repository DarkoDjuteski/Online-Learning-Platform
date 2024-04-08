using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLearningPlatform.GatewayApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CourseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: CourseController

        [HttpGet(Name = "Course")]
        public async Task<object> Get()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7231/Course");
            //HttpResponseMessage response = await _httpClient.GetAsync("http://gatewayapiservice/Course");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            // Process the response body as needed

            return responseBody;
        }

        // Other action methods...

    }
}
