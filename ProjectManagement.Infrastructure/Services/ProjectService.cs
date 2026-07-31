using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner.FirstName + " " + p.Owner.LastName,
                MemberCount = _context.ProjectMembers
                    .Count(pm => pm.ProjectId == p.Id)
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner.FirstName + " " + p.Owner.LastName,
                MemberCount = _context.ProjectMembers
                    .Count(pm => pm.ProjectId == p.Id)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        await ValidateProjectAsync(
            dto.Name,
            dto.Description,
            dto.Status,
            dto.StartDate,
            dto.DueDate,
            dto.OwnerId);

        var project = new Project
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Status = dto.Status.Trim(),
            StartDate = dto.StartDate,
            DueDate = dto.DueDate,
            OwnerId = dto.OwnerId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);

        await _context.SaveChangesAsync();

        var owner = await _context.Users
            .AsNoTracking()
            .FirstAsync(u => u.Id == project.OwnerId);

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status,
            StartDate = project.StartDate,
            DueDate = project.DueDate,
            CreatedAt = project.CreatedAt,
            OwnerId = project.OwnerId,
            OwnerName = $"{owner.FirstName} {owner.LastName}",
            MemberCount = 0
        };
    }

    public async Task<bool> UpdateAsync(int id, CreateProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project is null)
        {
            return false;
        }

        await ValidateProjectAsync(
            dto.Name,
            dto.Description,
            dto.Status,
            dto.StartDate,
            dto.DueDate,
            dto.OwnerId);

        project.Name = dto.Name.Trim();
        project.Description = dto.Description.Trim();
        project.Status = dto.Status.Trim();
        project.StartDate = dto.StartDate;
        project.DueDate = dto.DueDate;
        project.OwnerId = dto.OwnerId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project is null)
        {
            return false;
        }

        _context.Projects.Remove(project);

        await _context.SaveChangesAsync();

        return true;
    }

    private async System.Threading.Tasks.Task ValidateProjectAsync(
        string name,
        string description,
        string status,
        DateTime startDate,
        DateTime? dueDate,
        int ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ProjectValidationException(
                "Project name is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ProjectValidationException(
                "Project description is required.");
        }

        var validStatuses = new[]
        {
            "Active",
            "Completed",
            "On Hold",
            "Cancelled"
        };

        if (!validStatuses.Contains(status))
        {
            throw new ProjectValidationException(
                "Invalid project status. Allowed values: Active, Completed, On Hold, Cancelled.");
        }

        if (startDate == default)
        {
            throw new ProjectValidationException(
                "Project start date is required.");
        }

        if (dueDate.HasValue && dueDate.Value < startDate)
        {
            throw new ProjectValidationException(
                "Project due date cannot be earlier than the start date.");
        }

        var ownerExists = await _context.Users
            .AnyAsync(u => u.Id == ownerId);

        if (!ownerExists)
        {
            throw new ProjectValidationException(
                $"User with ID {ownerId} does not exist.");
        }
    }
}
