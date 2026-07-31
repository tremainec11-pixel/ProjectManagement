using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.ProjectMembers;
using ProjectManagement.Application.Services;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectMembersController : ControllerBase
{
private readonly IProjectMemberService _projectMemberService;

public ProjectMembersController(
    IProjectMemberService projectMemberService)
{
    _projectMemberService = projectMemberService;
}

[HttpGet("project/{projectId:int}")]
public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetByProjectId(
    int projectId)
{
    var members = await _projectMemberService
        .GetByProjectIdAsync(projectId);

    return Ok(members);
}

[HttpGet("{id:int}")]
public async Task<ActionResult<ProjectMemberDto>> GetById(int id)
{
    var member = await _projectMemberService.GetByIdAsync(id);

    if (member is null)
    {
        return NotFound();
    }

    return Ok(member);
}

[HttpPost]
public async Task<ActionResult<ProjectMemberDto>> Add(
    [FromBody] AddProjectMemberDto dto)
{
    var member = await _projectMemberService.AddAsync(dto);

    return CreatedAtAction(
        nameof(GetById),
        new { id = member.Id },
        member);
}

[HttpDelete("{id:int}")]
public async Task<IActionResult> Remove(int id)
{
    var removed = await _projectMemberService.RemoveAsync(id);

    if (!removed)
    {
        return NotFound();
    }

    return NoContent();
}


}
