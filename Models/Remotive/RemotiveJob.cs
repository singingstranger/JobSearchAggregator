namespace JobSearchAPI.Models.Remotive;

public class RemotiveJob
{
    public string Title { get; set; } = string.Empty;
    public string Company_Name { get; set; } = string.Empty;
    public string Candidate_Required_Location { get; set; } = string.Empty;
    public DateTime Publication_Date { get; set; }
}