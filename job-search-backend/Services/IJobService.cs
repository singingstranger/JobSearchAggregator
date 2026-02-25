using JobSearchAPI.Controllers;
using JobSearchAPI.Models;
namespace JobSearchAPI.Services;

public interface IJobService
{
    Task<IEnumerable<JobDTO>> SearchJobsAsynch(JobSearchRequest request);
}