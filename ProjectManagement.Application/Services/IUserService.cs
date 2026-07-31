using ProjectManagement.Application.DTOs.Users;

namespace ProjectManagement.Application.Services;

public interface IUserService
{
Task<IEnumerable<UserDto>> GetAllAsync();


Task<UserDto?> GetByIdAsync(int id);

Task<UserDto> CreateAsync(CreateUserDto dto);

Task<bool> UpdateAsync(int id, CreateUserDto dto);

Task<bool> DeleteAsync(int id);
 

}
