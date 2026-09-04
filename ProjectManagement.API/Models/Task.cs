namespace ProjectManagement.API.Models;

public class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedToId { get; set; }

    public string? AssignedToName { get; set; }
}