using System.Text.Json;
using JobSearchAPI.Controllers;
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
    private (decimal? Min, decimal? Max) ParseSalary(string? salaryText)
    {
        if (string.IsNullOrWhiteSpace(salaryText))
            return (null, null);

        salaryText = salaryText.Replace(",", "")
            .Replace("$", "")
            .Replace("€", "")
            .Replace("£", "")
            .ToLower();

        salaryText = salaryText.Replace("k", "000"); //because for some reason this keeps showing up

        var numbers = System.Text.RegularExpressions.Regex
            .Matches(salaryText, @"\d+")
            .Select(m => decimal.TryParse(m.Value, out var val) ? val : (decimal?)null)
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .ToList();

        if (numbers.Count == 0)
            return (null, null);

        if (numbers.Count == 1)
            return (numbers[0], numbers[0]);

        return (numbers.Min(), numbers.Max());
    }
    
    public async Task<IEnumerable<JobDTO>> SearchJobsAsync(JobSearchRequest request)
    {
        var url = $"https://remotive.com/api/remote-jobs?search={request.Keyword}";
        var response = await _httpClient.GetAsync(url);
        if(!response.IsSuccessStatusCode)
            return Enumerable.Empty<JobDTO>();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var content = await response.Content.ReadAsStringAsync();
        var remotiveResponse = JsonSerializer.Deserialize<RemotiveResponse>(content, options);

        if (remotiveResponse == null)
            return Enumerable.Empty<JobDTO>();
        
        var jobs = remotiveResponse.Jobs.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            jobs = jobs.Where(j =>
                j.Candidate_Required_Location.Contains(
                    request.Location,
                    StringComparison.OrdinalIgnoreCase));
        }

        return remotiveResponse.Jobs
            .Select(j => new JobDTO
            {
                Title = j.Title,
                Company = j.Company_Name,
                Location = j.Candidate_Required_Location,
                PostedDate = j.Publication_Date,
                MinSalary = null,
                MaxSalary = null,
                JobType = j.JobType,
                IsRemote = true,
                Source = "Remotive",
                OriginalURL = j.Url
            });
        
    }
}