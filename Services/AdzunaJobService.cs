using System.Text.Json;
using JobSearchAPI.Models;
using JobSearchAPI.Models.Adzuna;

namespace JobSearchAPI.Services;

public class AdzunaJobService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    public  AdzunaJobService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IEnumerable<JobDTO>> SearchJobsAsync(string keyword)
    {
        var appId = _configuration["Adzuna:AppID"];
        var appKey = _configuration["Adzuna:AppKey"];
        
        var url =
            $"https://api.adzuna.com/v1/api/jobs/gb/search/1" +
            $"?app_id={appId}" +
            $"&app_key={appKey}" +
            $"&what={Uri.EscapeDataString(keyword)}";
        
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return Enumerable.Empty<JobDTO>();

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var content = System.Text.Encoding.UTF8.GetString(bytes);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var adzunaResponse = JsonSerializer.Deserialize<AdzunaResponse>(content, options);
        if (adzunaResponse == null)
        {
            return Enumerable.Empty<JobDTO>();
        }

        var filteredJobs = adzunaResponse.Results.Where(j => !string.IsNullOrWhiteSpace(j.Redirect_Url));
        
        return filteredJobs
            .Select(j => new JobDTO
                {
                    Title = j.Title,
                    Company = j.Company.Display_Name,
                    Location = j.Location.Display_Name,
                    PostedDate = j.Created,
                    Source = "Adzuna",
                    OriginalURL = string.IsNullOrWhiteSpace(j.Redirect_Url) 
                        ? "https://www.adzuna.co.uk/" 
                        : j.Redirect_Url
                }
            );
    }
}