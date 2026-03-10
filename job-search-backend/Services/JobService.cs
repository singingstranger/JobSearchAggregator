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

        if (_cache.TryGetValue(cacheKey, out IEnumerable<JobDTO> cachedJobs))
        {
            return cachedJobs;
        }
        var tasks = _providers.Select(async p =>
        {
            try
            {
                return await p.SearchJobsAsync(request);
            }
            catch
            {
                Console.WriteLine("A job board API is returning exceptions.");
                return Enumerable.Empty<JobDTO>();
            }
        });
        
        await Task.WhenAll(tasks);

        var jobs = tasks.SelectMany(t => t.Result);
        
        // Filter duplicates
        jobs = jobs
            .GroupBy(j => j.OriginalURL)
            .Select(g => g.First());
        
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
        
        // Filter by days back
        if (request.DaysBack.HasValue)
        {
            var cutoff = DateTime.UtcNow.AddDays(-request.DaysBack.Value);
            jobs = jobs.Where(j => j.PostedDate >= cutoff);
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