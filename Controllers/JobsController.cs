using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _db;

    public JobsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/jobs
    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _db.Job
            .OrderByDescending(j => j.CreatedAt)
            .Take(100)
            .ToListAsync();

        return Ok(jobs);
    }
}