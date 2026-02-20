using JobSearchAPI.Models;

namespace JobSearchAPI.Services;

public class JobService : IJobService
{
    private readonly RemotiveJobServices _remotiveJobServices;
    private readonly AdzunaJobService _adzunaJobService;
    
    public JobService(RemotiveJobServices remotiveJobServices, AdzunaJobService adzunaJobService)
    {
        _remotiveJobServices = remotiveJobServices;
        _adzunaJobService = adzunaJobService;
    }
    public async Task<IEnumerable<JobDTO>> SearchJobsAsynch(string keyword, string location)
    {
        var remotiveTask = _remotiveJobServices.SearchJobsAsync(keyword);
        var adzunaTask = _adzunaJobService.SearchJobsAsync(keyword);

        var results = await Task.WhenAll(remotiveTask, adzunaTask);

        return results
            .SelectMany(r => r)
            .OrderByDescending(j => j.PostedDate);
    }
}