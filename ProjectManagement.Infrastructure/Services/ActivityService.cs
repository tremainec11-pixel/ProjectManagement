using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Activities;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class ActivityService : IActivityService
{
private readonly ApplicationDbContext _context;


public ActivityService(ApplicationDbContext context)
{
    _context = context;
}

public async Task<IEnumerable<ActivityDto>> GetAllAsync()
{
    return await _context.Activities
        .AsNoTracking()
        .OrderByDescending(a => a.CreatedAt)
        .Select(a => new ActivityDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            Type = a.Type,
            CreatedAt = a.CreatedAt
        })
        .ToListAsync();
}

public async Task<ActivityDto?> GetByIdAsync(int id)
{
    return await _context.Activities
        .AsNoTracking()
        .Where(a => a.Id == id)
        .Select(a => new ActivityDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            Type = a.Type,
            CreatedAt = a.CreatedAt
        })
        .FirstOrDefaultAsync();
}

public async Task<ActivityDto> CreateAsync(CreateActivityDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Title))
    {
        throw new ArgumentException(
            "Activity title is required.");
    }

    if (string.IsNullOrWhiteSpace(dto.Description))
    {
        throw new ArgumentException(
            "Activity description is required.");
    }

    if (string.IsNullOrWhiteSpace(dto.Type))
    {
        throw new ArgumentException(
            "Activity type is required.");
    }

    var activity = new ProjectManagement.Domain.Entities.Activity
    {
        Title = dto.Title,
        Description = dto.Description,
        Type = dto.Type,
        CreatedAt = DateTime.UtcNow
    };

    _context.Activities.Add(activity);

    await _context.SaveChangesAsync();

    return new ActivityDto
    {
        Id = activity.Id,
        Title = activity.Title,
        Description = activity.Description,
        Type = activity.Type,
        CreatedAt = activity.CreatedAt
    };
}

public async Task<bool> DeleteAsync(int id)
{
    var activity = await _context.Activities
        .FindAsync(id);

    if (activity is null)
    {
        return false;
    }

    _context.Activities.Remove(activity);

    await _context.SaveChangesAsync();

    return true;
}


}
