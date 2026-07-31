using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.ProjectMembers;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class ProjectMemberService : IProjectMemberService
{
    private readonly ApplicationDbContext _context;

    public ProjectMemberService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectMemberDto>> GetByProjectIdAsync(
        int projectId)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId);

        if (!projectExists)
        {
            throw new ProjectMemberValidationException(
                $"Project with ID {projectId} does not exist.");
        }

        return await _context.ProjectMembers
            .AsNoTracking()
            .Where(pm => pm.ProjectId == projectId)
            .Select(pm => new ProjectMemberDto
            {
                Id = pm.Id,
                ProjectId = pm.ProjectId,
                UserId = pm.UserId,
                Role = pm.Role,
                JoinedAt = pm.JoinedAt,
                UserName = pm.User.FirstName + " " + pm.User.LastName,
                Email = pm.User.Email
            })
            .ToListAsync();
    }

    public async Task<ProjectMemberDto?> GetByIdAsync(int id)
    {
        return await _context.ProjectMembers
            .AsNoTracking()
            .Where(pm => pm.Id == id)
            .Select(pm => new ProjectMemberDto
            {
                Id = pm.Id,
                ProjectId = pm.ProjectId,
                UserId = pm.UserId,
                Role = pm.Role,
                JoinedAt = pm.JoinedAt,
                UserName = pm.User.FirstName + " " + pm.User.LastName,
                Email = pm.User.Email
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProjectMemberDto> AddAsync(
        AddProjectMemberDto dto)
    {
        if (dto.ProjectId <= 0)
        {
            throw new ProjectMemberValidationException(
                "Project ID must be greater than zero.");
        }

        if (dto.UserId <= 0)
        {
            throw new ProjectMemberValidationException(
                "User ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Role))
        {
            throw new ProjectMemberValidationException(
                "Member role is required.");
        }

        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == dto.ProjectId);

        if (!projectExists)
        {
            throw new ProjectMemberValidationException(
                $"Project with ID {dto.ProjectId} does not exist.");
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);

        if (user is null)
        {
            throw new ProjectMemberValidationException(
                $"User with ID {dto.UserId} does not exist.");
        }

        var alreadyMember = await _context.ProjectMembers
            .AnyAsync(pm =>
                pm.ProjectId == dto.ProjectId &&
                pm.UserId == dto.UserId);

        if (alreadyMember)
        {
            throw new ProjectMemberValidationException(
                $"User with ID {dto.UserId} is already a member of project with ID {dto.ProjectId}.");
        }

        var projectMember = new ProjectMember
        {
            ProjectId = dto.ProjectId,
            UserId = dto.UserId,
            Role = dto.Role.Trim(),
            JoinedAt = DateTime.UtcNow
        };

        _context.ProjectMembers.Add(projectMember);

        await _context.SaveChangesAsync();

        return new ProjectMemberDto
        {
            Id = projectMember.Id,
            ProjectId = projectMember.ProjectId,
            UserId = projectMember.UserId,
            Role = projectMember.Role,
            JoinedAt = projectMember.JoinedAt,
            UserName = $"{user.FirstName} {user.LastName}",
            Email = user.Email
        };
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var projectMember = await _context.ProjectMembers
            .FindAsync(id);

        if (projectMember is null)
        {
            return false;
        }

        _context.ProjectMembers.Remove(projectMember);

        await _context.SaveChangesAsync();

        return true;
    }
}
