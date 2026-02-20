namespace JobSearchAPI.Models.Adzuna;

public class AdzunaJob
{
    public string Title { get; set; } = string.Empty;
    public AdzunaCompany Company { get; set; } = new();
    public AdzunaLocation Location { get; set; } = new();
    public DateTime Created { get; set; }
    public string Redirect_Url { get; set; } 
}
public class AdzunaCompany
{
    public string Display_Name { get; set; } = string.Empty;
}
public class AdzunaLocation
{
    public string Display_Name { get; set; } = string.Empty;
}