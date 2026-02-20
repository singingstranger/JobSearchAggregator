using System.Text.Json;
using JobSearchAPI.Models;
using JobSearchAPI.Models.Remotive;

namespace JobSearchAPI.Services;

public class RemotiveJobServices
{
    private readonly HttpClient _httpClient;
    
    public RemotiveJobServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<JobDTO>> SearchJobsAsync(string keyword)
    {
        var url = $"https://remotive.com/api/remote-jobs?search={keyword}";
        var response = await _httpClient.GetAsync(url);
        if(!response.IsSuccessStatusCode)
            return Enumerable.Empty<JobDTO>();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var content = await response.Content.ReadAsStringAsync();
        var remotiveResponse = JsonSerializer.Deserialize<RemotiveResponse>(content, options);

        if (remotiveResponse == null)
            return Enumerable.Empty<JobDTO>();

        return remotiveResponse.Jobs
            .Where(j => j.Publication_Date.Date >= DateTime.UtcNow.AddDays(-10))
            .Select(j => new JobDTO
            {
                Title = j.Title,
                Company = j.Company_Name,
                Location = j.Candidate_Required_Location,
                PostedDate = j.Publication_Date,
                Source = "Remotive",
                OriginalURL = j.Url
            });
        
    }
}