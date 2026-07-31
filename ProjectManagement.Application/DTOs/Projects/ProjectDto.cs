namespace ProjectManagement.Application.DTOs.Projects;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }

    public string OwnerName { get; set; } = string.Empty;

    public int MemberCount { get; set; }
}