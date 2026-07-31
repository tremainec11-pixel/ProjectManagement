namespace ProjectManagement.Domain.Entities;

public class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "To Do";

    public string Priority { get; set; } = "Medium";

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public int? AssignedToId { get; set; }

    public User? AssignedTo { get; set; }
}