using ProjectManagement.Application.DTOs.ProjectMembers;

namespace ProjectManagement.Application.Services;

public interface IProjectMemberService
{
Task<IEnumerable<ProjectMemberDto>> GetByProjectIdAsync(int projectId);

Task<ProjectMemberDto?> GetByIdAsync(int id);

Task<ProjectMemberDto> AddAsync(AddProjectMemberDto dto);

Task<bool> RemoveAsync(int id);

}
