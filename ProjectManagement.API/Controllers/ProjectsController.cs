using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        var projects = await (
            from p in _context.Projects
            join u in _context.Users
                on p.OwnerId equals u.Id into users
            from u in users.DefaultIfEmpty()
            select new Project
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerName = u != null
                    ? u.FirstName + " " + u.LastName
                    : "",
                MemberCount = _context.ProjectMembers
                    .Count(pm => pm.ProjectId == p.Id)
            }
        ).ToListAsync();

        return projects;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await (
            from p in _context.Projects
            join u in _context.Users
                on p.OwnerId equals u.Id into users
            from u in users.DefaultIfEmpty()
            where p.Id == id
            select new Project
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerName = u != null
                    ? u.FirstName + " " + u.LastName
                    : "",
                MemberCount = _context.ProjectMembers
                    .Count(pm => pm.ProjectId == p.Id)
            }
        ).FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound();
        }

        return project;
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProject),
            new { id = project.Id },
            project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, Project project)
    {
        if (id != project.Id)
        {
            return BadRequest();
        }

        _context.Entry(project).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}