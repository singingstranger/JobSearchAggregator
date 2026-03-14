namespace JobSearchAPI.Models;

public class JobDTO
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public string Source { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string JobType { get; set; } = string.Empty;
    public bool IsRemote { get; set; }
}