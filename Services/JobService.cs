using JobSearchAPI.Models;

namespace JobSearchAPI.Services;

public class JobService : IJobService
{
    public async Task<IEnumerable<JobDTO>> SearchJobsAsynch(string keyword, string location)
    {
        //simulation, todo: integrate real search
        await Task.Delay(500);
        return new List<JobDTO>
        {
            new JobDTO
            {
                Title = $"{keyword} Developer",
                Company = "Imaginary Company",
                Location = location,
                PostedDate = DateTime.UtcNow
            }
        };
    }
}