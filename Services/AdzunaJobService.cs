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
        var appID = _configuration["Adzuna:AppID"];
        var appKey = _configuration["Adzuna:Key"];
        
        var url = 
            $"https://api.adzuna.com/v1/api/jobs/gb/search/1" +
            $"?app_id={appID}" +
            $"&app_key={appKey}" +
            $"&what={keyword}" +
            $"&content-type=application/json";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return Enumerable.Empty<JobDTO>();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var adzunaResponse = JsonSerializer.Deserialize<AdzunaResponse>(content, options);
        if (adzunaResponse == null)
        {
            return Enumerable.Empty<JobDTO>();
        }

        return adzunaResponse.Results
            .Select(j => new JobDTO
                {
                    Title = j.Title,
                    Company = j.Company.Display_Name,
                    Location = j.Location.Display_Name,
                    PostedDate = j.Created
                        
                }
            );
    }
}