namespace ProjectManagement.Application.DTOs.ProjectMembers;

public class AddProjectMemberDto
{
public int ProjectId { get; set; }

public int UserId { get; set; }

public string Role { get; set; } = "Member";

}
