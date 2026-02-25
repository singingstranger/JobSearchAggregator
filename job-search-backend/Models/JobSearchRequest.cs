namespace JobSearchAPI.Controllers;

public class JobSearchRequest
{
    public string Keyword { get; set; } = string.Empty;
    public string? Location { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public int? DaysBack { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
}