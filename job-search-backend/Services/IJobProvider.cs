using JobSearchAPI.Models;
using JobSearchAPI.Controllers;
namespace JobSearchAPI.job_search_backend.Services;

public interface IJobProvider
{
    Task<IEnumerable<JobDTO>> SearchJobsAsync(JobSearchRequest request);
}


/* This interface allows for quick addition of further API's.
 Example:

public class ExampleJobService : IJobProvider
{
private readonly HttpClient _httpClient;

public ReedJobService(HttpClient httpClient)
{
    _httpClient = httpClient;
}

public async Task<IEnumerable<JobDTO>> SearchJobsAsync(JobSearchRequest request)
{
    var url = $"https://www.example.com/api/jobs?keywords={request.Keyword}";

    var response = await _httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return Enumerable.Empty<JobDTO>();

    var content = await response.Content.ReadAsStringAsync();

    // JSON parsing here...

    return jobs;
}
}

Then, in program.cs, add:
builder.Services.AddScoped<IJobProvider, ReedJobService>();
*/