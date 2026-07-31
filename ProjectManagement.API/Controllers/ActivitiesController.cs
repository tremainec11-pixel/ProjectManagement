using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Activities;
using ProjectManagement.Application.Interfaces;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
private readonly IActivityService _activityService;


public ActivitiesController(IActivityService activityService)
{
    _activityService = activityService;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    var activities = await _activityService.GetAllAsync();

    return Ok(activities);
}

[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var activity = await _activityService.GetByIdAsync(id);

    if (activity is null)
    {
        return NotFound(new
        {
            message = "Activity not found."
        });
    }

    return Ok(activity);
}

[HttpPost]
public async Task<IActionResult> Create(CreateActivityDto dto)
{
    var activity = await _activityService.CreateAsync(dto);

    return CreatedAtAction(
        nameof(GetById),
        new { id = activity.Id },
        activity);
}

[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    var deleted = await _activityService.DeleteAsync(id);

    if (!deleted)
    {
        return NotFound(new
        {
            message = "Activity not found."
        });
    }

    return NoContent();
}


}
