using JobSearchAPI.Models;

namespace JobSearchAPI.Services;

public class JobService : IJobService
{
    private readonly RemotiveJobServices _remotiveJobServices;

    public JobService(RemotiveJobServices remotiveJobServices)
    {
        _remotiveJobServices = remotiveJobServices;
    }
    public async Task<IEnumerable<JobDTO>> SearchJobsAsynch(string keyword, string location)
    {
       var remotiveJobs = await _remotiveJobServices.SearchJobsAsync(keyword);
       return remotiveJobs;
    }
}