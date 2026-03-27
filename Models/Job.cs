public class Job
{
    public int Id { get; set; }
    public int ExternalId { get; set; }
    public string Title { get; set; }
    public int CompanyId { get; set; }
    public string Url { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}