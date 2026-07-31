using ProjectManagement.Application.DTOs.Projects;

namespace ProjectManagement.Application.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();

    Task<ProjectDto?> GetByIdAsync(int id);

    Task<ProjectDto> CreateAsync(CreateProjectDto dto);

    Task<bool> UpdateAsync(int id, CreateProjectDto dto);

    Task<bool> DeleteAsync(int id);
}