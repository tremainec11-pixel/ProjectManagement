using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ActivitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
    {
        return await _context.Activities
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivity(int id)
    {
        var activity = await _context.Activities.FindAsync(id);

        if (activity == null)
            return NotFound();

        return activity;
    }

    [HttpPost]
    public async Task<ActionResult<Activity>> CreateActivity(Activity activity)
    {
        activity.CreatedAt = DateTime.UtcNow;

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetActivity),
            new { id = activity.Id },
            activity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var activity = await _context.Activities.FindAsync(id);

        if (activity == null)
            return NotFound();

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}