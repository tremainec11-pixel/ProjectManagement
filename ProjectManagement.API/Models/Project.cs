namespace ProjectManagement.API.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }

    public string OwnerName { get; set; } = string.Empty;

    public int MemberCount { get; set; }
}