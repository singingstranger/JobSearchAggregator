using System.Text.Json.Serialization;

namespace JobSearchAPI.job_search_backend.Models.Remotive;

public class RemotiveJob
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("company_name")]
    public string Company { get; set; } = string.Empty;

    [JsonPropertyName("candidate_required_location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("publication_date")]
    public DateTime PublicationDate { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("salary")]
    public string? Salary { get; set; }

    [JsonPropertyName("job_type")]
    public string? JobType { get; set; }
}