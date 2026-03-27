using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CompaniesController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/companies
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _db.Company
            .Where(c => c.IsActive)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return Ok(companies);
    }
}