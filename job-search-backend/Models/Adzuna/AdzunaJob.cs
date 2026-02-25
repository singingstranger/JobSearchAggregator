using System.Text.Json.Serialization;
namespace JobSearchAPI.Models.Adzuna;

public class AdzunaJob
{
    public string Title { get; set; } = string.Empty;
    public AdzunaCompany Company { get; set; } = new();
    public AdzunaLocation Location { get; set; } = new();
    public DateTime Created { get; set; }
    
    [JsonPropertyName("redirect_url")]
    public string RedirectUrl { get; set; } 
    
    [JsonPropertyName("salary_min")]
    public decimal? SalaryMin { get; set; }

    [JsonPropertyName("salary_max")]
    public decimal? SalaryMax { get; set; }

    [JsonPropertyName("contract_type")]
    public string? ContractType { get; set; }
}
public class AdzunaCompany
{
    public string Display_Name { get; set; } = string.Empty;
}
public class AdzunaLocation
{
    public string Display_Name { get; set; } = string.Empty;
}