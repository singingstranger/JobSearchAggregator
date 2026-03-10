using JobSearchAPI.job_search_backend.Models.Remotive;
using System.Text.Json.Serialization;

namespace JobSearchAPI.Models.Remotive;

public class RemotiveResponse
{
    [JsonPropertyName("jobs")]
    public List<RemotiveJob> Jobs { get; set; } = new();
}