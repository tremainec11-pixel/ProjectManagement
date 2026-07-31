using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.Services;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
private readonly IProjectService _projectService;


public ProjectsController(IProjectService projectService)
{
    _projectService = projectService;
}

[HttpGet]
public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
{
    var projects = await _projectService.GetAllAsync();

    return Ok(projects);
}

[HttpGet("{id:int}")]
public async Task<ActionResult<ProjectDto>> GetById(int id)
{
    var project = await _projectService.GetByIdAsync(id);

    if (project is null)
    {
        return NotFound();
    }

    return Ok(project);
}

[HttpPost]
public async Task<ActionResult<ProjectDto>> Create(
    [FromBody] CreateProjectDto dto)
{
    var project = await _projectService.CreateAsync(dto);

    return CreatedAtAction(
        nameof(GetById),
        new { id = project.Id },
        project);
}

[HttpPut("{id:int}")]
public async Task<IActionResult> Update(
    int id,
    [FromBody] CreateProjectDto dto)
{
    var updated = await _projectService.UpdateAsync(id, dto);

    if (!updated)
    {
        return NotFound();
    }

    return NoContent();
}

[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    var deleted = await _projectService.DeleteAsync(id);

    if (!deleted)
    {
        return NotFound();
    }

    return NoContent();
}


}
