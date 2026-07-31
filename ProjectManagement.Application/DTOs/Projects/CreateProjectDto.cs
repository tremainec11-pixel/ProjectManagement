namespace ProjectManagement.Application.DTOs.Projects;

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "Active";

    public DateTime StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public int OwnerId { get; set; }
}