using JobSearchAPI.Models;
namespace JobSearchAPI.Services;

public interface IJobService
{
    Task<IEnumerable<JobDTO>> SearchJobsAsynch(string keyword, string location);
}