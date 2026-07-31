using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Users;
using ProjectManagement.Application.Services;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class UserService : IUserService
{
private readonly ApplicationDbContext _context;

public UserService(ApplicationDbContext context)
{
    _context = context;
}

public async Task<IEnumerable<UserDto>> GetAllAsync()
{
    return await _context.Users
        .AsNoTracking()
        .Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role,
            CreatedAt = u.CreatedAt,
            IsActive = u.IsActive
        })
        .ToListAsync();
}

public async Task<UserDto?> GetByIdAsync(int id)
{
    return await _context.Users
        .AsNoTracking()
        .Where(u => u.Id == id)
        .Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.Role,
            CreatedAt = u.CreatedAt,
            IsActive = u.IsActive
        })
        .FirstOrDefaultAsync();
}

public async Task<UserDto> CreateAsync(CreateUserDto dto)
{
    var user = new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PasswordHash = string.Empty,
        Role = dto.Role,
        CreatedAt = DateTime.UtcNow,
        IsActive = true
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    return new UserDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role,
        CreatedAt = user.CreatedAt,
        IsActive = user.IsActive
    };
}

public async Task<bool> UpdateAsync(int id, CreateUserDto dto)
{
    var user = await _context.Users.FindAsync(id);

    if (user is null)
    {
        return false;
    }

    user.FirstName = dto.FirstName;
    user.LastName = dto.LastName;
    user.Email = dto.Email;
    user.Role = dto.Role;

    await _context.SaveChangesAsync();

    return true;
}

public async Task<bool> DeleteAsync(int id)
{
    var user = await _context.Users.FindAsync(id);

    if (user is null)
    {
        return false;
    }

    _context.Users.Remove(user);

    await _context.SaveChangesAsync();

    return true;
}


}
