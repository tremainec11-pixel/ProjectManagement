using ProjectManagement.Application.DTOs.Activities;

namespace ProjectManagement.Application.Interfaces;

public interface IActivityService
{
Task<IEnumerable<ActivityDto>> GetAllAsync();


Task<ActivityDto?> GetByIdAsync(int id);

Task<ActivityDto> CreateAsync(CreateActivityDto dto);

Task<bool> DeleteAsync(int id);


}
