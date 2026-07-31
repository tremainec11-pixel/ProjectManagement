using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================

    public async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        return await _context.Tasks
            .AsNoTracking()
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                ProjectId = t.ProjectId,
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo != null
                    ? t.AssignedTo.FirstName + " " + t.AssignedTo.LastName
                    : null
            })
            .ToListAsync();
    }


    // =========================
    // GET BY ID
    // =========================

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                ProjectId = t.ProjectId,
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo != null
                    ? t.AssignedTo.FirstName + " " + t.AssignedTo.LastName
                    : null
            })
            .FirstOrDefaultAsync();
    }


    // =========================
    // CREATE
    // =========================

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        await ValidateTaskAsync(
            dto.Title,
            dto.Description,
            dto.Status,
            dto.Priority,
            dto.ProjectId,
            dto.AssignedToId);

        var task = new ProjectManagement.Domain.Entities.Task
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
            AssignedToId = dto.AssignedToId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);

        await _context.SaveChangesAsync();

        var assignedUser = dto.AssignedToId.HasValue
            ? await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.AssignedToId.Value)
            : null;

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            AssignedToName = assignedUser != null
                ? $"{assignedUser.FirstName} {assignedUser.LastName}"
                : null
        };
    }


    // =========================
    // UPDATE
    // =========================

    public async Task<bool> UpdateAsync(
        int id,
        UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        await ValidateTaskAsync(
            dto.Title,
            dto.Description,
            dto.Status,
            dto.Priority,
            dto.ProjectId,
            dto.AssignedToId);

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.ProjectId = dto.ProjectId;
        task.AssignedToId = dto.AssignedToId;

        await _context.SaveChangesAsync();

        return true;
    }


    // =========================
    // DELETE
    // =========================

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return true;
    }


    // =========================
    // VALIDATION
    // =========================

    private async Task ValidateTaskAsync(
        string title,
        string description,
        string status,
        string priority,
        int projectId,
        int? assignedToId)
    {
        // Validate title

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TaskValidationException(
                "Task title is required.");
        }


        // Validate description

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new TaskValidationException(
                "Task description is required.");
        }


        // Validate status

        var validStatuses = new[]
        {
            "Todo",
            "In Progress",
            "Done"
        };

        if (!validStatuses.Contains(status))
        {
            throw new TaskValidationException(
                "Invalid task status. Allowed values: Todo, In Progress, Done.");
        }


        // Validate priority

        var validPriorities = new[]
        {
            "Low",
            "Medium",
            "High"
        };

        if (!validPriorities.Contains(priority))
        {
            throw new TaskValidationException(
                "Invalid task priority. Allowed values: Low, Medium, High.");
        }


        // Validate project

        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId);

        if (!projectExists)
        {
            throw new TaskValidationException(
                $"Project with ID {projectId} does not exist.");
        }


        // Validate assigned user

        if (assignedToId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == assignedToId.Value);

            if (!userExists)
            {
                throw new TaskValidationException(
                    $"User with ID {assignedToId.Value} does not exist.");
            }
        }
    }
}