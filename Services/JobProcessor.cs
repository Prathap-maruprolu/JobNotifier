public class JobProcessor
{
    private readonly AppDbContext _db;
    private readonly NotificationService _notification;

    public JobProcessor(AppDbContext db, NotificationService notification)
    {
        _db = db;
        _notification = notification;
    }

    public async Task ProcessJobs(List<Job> jobs, int companyId, string companyName)
    {
        var existingIds = _db.Job.Select(j => j.ExternalId).ToHashSet();

        foreach (var job in jobs)
        {
            if (!existingIds.Contains(job.ExternalId))
            {
                job.CompanyId = companyId;

                _db.Job.Add(job);
                await _notification.Send(job, companyName);
                
            }
        }

        await _db.SaveChangesAsync();
    }
}