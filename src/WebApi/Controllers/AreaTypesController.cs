using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Controllers;

[ApiController]
[Route("api/area-types")]
public class AreaTypesController : ControllerBase
{
    private readonly AdleDbContext _db;

    public AreaTypesController(AdleDbContext db) => _db = db;

    public record AreaTypeDto(int Id, string? Name, string? Definition);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaTypeDto>>> GetAll(CancellationToken ct)
    {
        var rows = await _db.AreaTypes
            .AsNoTracking()
            .Where(t => t.DeletedDate == null)
            .OrderBy(t => t.ID)
            .Select(t => new AreaTypeDto(t.ID, t.Name, t.Definition))
            .ToListAsync(ct);

        return Ok(rows);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AreaTypeDto>> GetById(int id, CancellationToken ct)
    {
        var row = await _db.AreaTypes
            .AsNoTracking()
            .Where(t => t.ID == id)
            .Select(t => new AreaTypeDto(t.ID, t.Name, t.Definition))
            .FirstOrDefaultAsync(ct);

        return row is null ? NotFound() : Ok(row);
    }
}
