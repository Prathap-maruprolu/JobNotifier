using System.Text.Json;

public class GreenhouseService
{
    private readonly HttpClient _httpClient;

    public GreenhouseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Job>> GetJobs(string companyKey, int companyId)
    {
        try 
        {
             return await FetchJobs(companyKey, companyId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching jobs for {companyKey}: {ex.Message}");
            return new List<Job>(); 
        }
    }

    private async Task<List<Job>> FetchJobs(string companyKey, int companyId)
    {

        var url = $"https://api.greenhouse.io/v1/boards/{companyKey}/jobs";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failed for {companyKey}: {response.StatusCode}");
            return new List<Job>(); 
        }

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var jobs = new List<Job>();

        foreach (var job in json.RootElement.GetProperty("jobs").EnumerateArray())
        {
            var updatedAt = job.GetProperty("updated_at").GetDateTime();

            updatedAt = DateTime.SpecifyKind(updatedAt, DateTimeKind.Utc);

            if (updatedAt < DateTime.UtcNow.AddHours(-10))
                continue;
            

            jobs.Add(new Job
            {
                ExternalId = job.GetProperty("id").GetInt32(),
                Title = job.GetProperty("title").GetString(),
                Url = job.GetProperty("absolute_url").GetString(),
                CompanyId = companyId,
                Description = job.TryGetProperty("content", out var descProp) ? descProp.GetString() : null,
                Location = job.TryGetProperty("location", out var locProp) && locProp.TryGetProperty("name", out var nameProp)
                            ? nameProp.GetString()
                            : null,
                CreatedAt = updatedAt
            });
        }

        return jobs;
    }
}