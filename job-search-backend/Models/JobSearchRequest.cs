using System.ComponentModel.DataAnnotations;

namespace JobSearchAPI.Controllers;

public class JobSearchRequest
{
    [MaxLength(100)]
    public string Keyword { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? Location { get; set; }
    
    [Range(1,100)]
    public int Page { get; set; } = 1;
    [Range(1,50)]
    public int PageSize { get; set; } = 10;
    
    [Range(1,30)]
    public int? DaysBack { get; set; }
    [Range(0,10000000)]
    public decimal? MinSalary { get; set; }
    [Range(0,10000000)]
    public decimal? MaxSalary { get; set; }
}