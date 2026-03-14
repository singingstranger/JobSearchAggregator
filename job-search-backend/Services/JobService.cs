using JobSearchAPI.Controllers;
using JobSearchAPI.job_search_backend.Services;
using JobSearchAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace JobSearchAPI.Services;

public class JobService : IJobService
{
    private readonly IEnumerable<IJobProvider> _providers;
    private readonly IMemoryCache _cache;
    
    public JobService(IEnumerable<IJobProvider> providers, IMemoryCache cache)
    {
        _providers = providers ?? Enumerable.Empty<IJobProvider>();
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

        if (_cache.TryGetValue(cacheKey, out IEnumerable<JobDTO>? cachedJobs) && cachedJobs != null)
        {
            return cachedJobs;
        }
        var tasks = _providers.Select(provider =>
            Task.Run(async () =>
            {
                try
                {
                    var timeout = Task.Delay(5000);
                    var work = provider.SearchJobsAsync(request);

                    var finished = await Task.WhenAny(work, timeout);

                    if (finished == timeout)
                    {
                        Console.WriteLine($"{provider.GetType().Name} timed out.");
                        return Enumerable.Empty<JobDTO>();
                    }

                    return await work;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{provider.GetType().Name} failed: {ex.Message}");
                    return Enumerable.Empty<JobDTO>();
                }
            })
        );

        var results = await Task.WhenAll(tasks);
        var jobs = results.SelectMany(j => j);
        
        // Filter duplicates
        jobs = jobs
            .GroupBy(j => j.OriginalUrl)
            .Select(g => g.First());
        
        // Filter by days back
        if (request.DaysBack.HasValue)
        {
            var cutoff = DateTime.UtcNow.AddDays(-request.DaysBack.Value);
            jobs = jobs.Where(j =>
            {
                var posted = 
                    j.PostedDate.Kind == DateTimeKind.Utc 
                        ? j.PostedDate 
                        : j.PostedDate.ToUniversalTime();
                return posted >= cutoff;
            });
        }
        
        // Keyword filtering
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            jobs = jobs.Where(j =>
                j.Title.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                j.Company.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase));
        }
        
        // Filter location
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            jobs = jobs.Where(j =>
                j.IsRemote ||
                j.Location.Contains(request.Location, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by salary
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
        
        // Sort by posted date
        jobs = jobs
            .OrderByDescending(j => j.PostedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

            
        _cache.Set(cacheKey, jobs, TimeSpan.FromMinutes(5));
        return jobs;
    }
}