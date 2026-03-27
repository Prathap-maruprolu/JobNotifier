using Microsoft.Extensions.Hosting;

public class JobBackgroundService : BackgroundService
{
    private readonly IServiceProvider _sp;

    public JobBackgroundService(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var greenhouse = scope.ServiceProvider.GetRequiredService<GreenhouseService>();
            var processor = scope.ServiceProvider.GetRequiredService<JobProcessor>();

            var companies = db.Company.Where(c => c.IsActive).ToList();

            foreach (var company in companies)
            {
                try
                {
                    var jobs = await greenhouse.GetJobs(company.GreenhouseKey, company.Id);
                    await processor.ProcessJobs(jobs, company.Id, company.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Company failed: {company.Name} - {ex.Message}");
                }
            }

           await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}