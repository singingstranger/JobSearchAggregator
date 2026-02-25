using JobSearchAPI.Controllers;
using JobSearchAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace JobSearchAPI.Services;

public class JobService : IJobService
{
    private readonly RemotiveJobServices _remotiveJobServices;
    private readonly AdzunaJobService _adzunaJobService;
    private readonly IMemoryCache _cache;
    
    public JobService(RemotiveJobServices remotiveJobServices, AdzunaJobService adzunaJobService, IMemoryCache cache)
    {
        _remotiveJobServices = remotiveJobServices;
        _adzunaJobService = adzunaJobService;
        _cache = cache;
    }
    public async Task<IEnumerable<JobDTO>> SearchJobsAsynch(JobSearchRequest request)
    {
        var cacheKey = $"jobs_" +
                       $"{request.Keyword}_" +
                       $"{request.Location}_" +
                       $"{request.Page}_" +
                       $"{request.PageSize}_" +
                       $"{request.MinSalary}_" +
                       $"{request.MaxSalary}_" +
                       $"{request.DaysBack}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<JobDTO> cachedJobs))
        {
            return cachedJobs;
        }
        
        var remotiveTask = _remotiveJobServices.SearchJobsAsync(request);
        var adzunaTask = _adzunaJobService.SearchJobsAsync(request);

        await Task.WhenAll(remotiveTask, adzunaTask);

        var jobs = remotiveTask.Result
            .Concat(adzunaTask.Result);

        // Filter by days back
        if (request.DaysBack.HasValue)
        {
            var cutoff = DateTime.UtcNow.AddDays(-request.DaysBack.Value);
            jobs = jobs.Where(j => j.PostedDate >= cutoff);
        }

        if (request.MinSalary.HasValue)
        {
            jobs = jobs.Where(j =>
                j.MaxSalary.HasValue &&
                j.MaxSalary.Value >= request.MinSalary.Value);
        }

        if (request.MaxSalary.HasValue)
        {
            jobs = jobs.Where(j =>
                j.MinSalary.HasValue &&
                j.MinSalary.Value <= request.MaxSalary.Value);
        }
        jobs = jobs
            .GroupBy(j => new { j.Title, j.Company })
            .Select(g => g.First());
        
        jobs = jobs
            .OrderByDescending(j => j.PostedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
            
        _cache.Set(cacheKey, jobs, TimeSpan.FromMinutes(5));
        return jobs;
    }
}