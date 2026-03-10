using System.Text.Json;
using JobSearchAPI.Controllers;
using JobSearchAPI.job_search_backend.Services;
using JobSearchAPI.Models;
using JobSearchAPI.Models.Adzuna;

namespace JobSearchAPI.Services;

public class AdzunaJobService : IJobProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    private static readonly ProviderRateLimiter _limiter =
        new(TimeSpan.FromSeconds(5));
    
    public  AdzunaJobService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IEnumerable<JobDTO>> SearchJobsAsync(JobSearchRequest request)
    {
        var appId = _configuration["Adzuna:AppID"];
        var appKey = _configuration["Adzuna:AppKey"];
        
        var url =
            $"https://api.adzuna.com/v1/api/jobs/gb/search/1" +
            $"?app_id={appId}" +
            $"&app_key={appKey}" +
            $"&what={Uri.EscapeDataString(request.Keyword ?? "")}" +
            $"&where={Uri.EscapeDataString(request.Location ?? "")}" +
            $"&max_days_old={(request.DaysBack > 0 ? request.DaysBack : 3)}" +
            $"&results_per_page={(request.PageSize > 0 ? request.PageSize : 10)}";
        
        await _limiter.WaitAsync();
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

        var filteredJobs = adzunaResponse.Results.Where(j => !string.IsNullOrWhiteSpace(j.RedirectUrl));
        
        return filteredJobs
            .Select(j => new JobDTO
            {
                Title = j.Title?.Trim() ?? "Unknown job",
                Company = j.Company?.Display_Name?.Trim() ?? "Unknown company",
                Location = j.Location?.Display_Name?.Trim() ?? "Remote",
                PostedDate = j.Created,
                MinSalary = j.SalaryMin,
                MaxSalary = j.SalaryMax,
                JobType = j.ContractType?.Trim() ?? "Unknown type",
                IsRemote = j.Location?.Display_Name?.Contains("Remote", StringComparison.OrdinalIgnoreCase) ?? false,
                Source = "Adzuna",
                OriginalURL = string.IsNullOrWhiteSpace(j.RedirectUrl) 
                    ? "https://www.adzuna.co.uk/" 
                    : j.RedirectUrl.Trim()
            });
    }
}