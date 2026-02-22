using Microsoft.AspNetCore.Mvc;
using JobSearchAPI.Services;

namespace JobSearchAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] JobSearchRequest request)
    {
        var results = await _jobService.SearchJobsAsynch(request);
        return Ok(results);
    }

/*[HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string keyword,
        [FromQuery] string location)
    {
        var results = await _jobService.SearchJobsAsynch(keyword, location);
        return Ok(results);
    }*/
}