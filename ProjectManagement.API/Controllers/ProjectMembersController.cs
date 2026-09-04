using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectMembersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectMembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<ProjectMember>>> GetProjectMembers(int projectId)
    {
        var members = await (
            from pm in _context.ProjectMembers
            join u in _context.Users
                on pm.UserId equals u.Id
            where pm.ProjectId == projectId
            select new ProjectMember
            {
                Id = pm.Id,
                ProjectId = pm.ProjectId,
                UserId = pm.UserId,
                UserName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                Role = pm.Role,
                JoinedAt = pm.JoinedAt
            }
        ).ToListAsync();

        return members;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectMember>> GetProjectMember(int id)
    {
        var member = await (
            from pm in _context.ProjectMembers
            join u in _context.Users
                on pm.UserId equals u.Id
            where pm.Id == id
            select new ProjectMember
            {
                Id = pm.Id,
                ProjectId = pm.ProjectId,
                UserId = pm.UserId,
                UserName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                Role = pm.Role,
                JoinedAt = pm.JoinedAt
            }
        ).FirstOrDefaultAsync();

        if (member == null)
            return NotFound();

        return member;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMember>> CreateProjectMember(
        ProjectMember projectMember)
    {
        var user = await _context.Users
            .FindAsync(projectMember.UserId);

        if (user == null)
        {
            return BadRequest(new
            {
                message = "User not found."
            });
        }

        var project = await _context.Projects
            .FindAsync(projectMember.ProjectId);

        if (project == null)
        {
            return BadRequest(new
            {
                message = "Project not found."
            });
        }

        var alreadyExists = await _context.ProjectMembers
            .AnyAsync(pm =>
                pm.ProjectId == projectMember.ProjectId &&
                pm.UserId == projectMember.UserId);

        if (alreadyExists)
        {
            return Conflict(new
            {
                message = "This user is already a member of this project."
            });
        }

        projectMember.UserName =
            $"{user.FirstName} {user.LastName}";

        projectMember.Email = user.Email;

        projectMember.JoinedAt = DateTime.UtcNow;

        _context.ProjectMembers.Add(projectMember);

        await _context.SaveChangesAsync();

        projectMember.UserName =
            $"{user.FirstName} {user.LastName}";

        projectMember.Email = user.Email;

        return CreatedAtAction(
            nameof(GetProjectMember),
            new { id = projectMember.Id },
            projectMember);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjectMember(int id)
    {
        var member = await _context.ProjectMembers
            .FindAsync(id);

        if (member == null)
            return NotFound();

        _context.ProjectMembers.Remove(member);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}