namespace ProjectManagement.Application.DTOs.Tasks;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "To Do";

    public string Priority { get; set; } = "Medium";

    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedToId { get; set; }
}