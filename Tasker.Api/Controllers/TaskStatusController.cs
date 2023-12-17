using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Application.Interfaces.Services;

namespace Tasker.Controllers;

[ApiController]
[Route("api/taskStatus")]
public class TaskStatusController : ControllerBase
{
    private readonly ITaskStatusService _service;

    public TaskStatusController(ITaskStatusService service)
    {
        _service = service;
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [Authorize(Roles = "User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var dto = await _service.GetByIdAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TaskStatusCreateDto dto)
    {
        var createdDto = await _service.CreateAsync(dto);

        return Ok(createdDto);
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TaskStatusUpdateDto dto)
    {
        var updatedDto = await _service.UpdateAsync(dto);

        return Ok(updatedDto);
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deleted = await _service.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new { error = $"Task status with id {id} does not exist" });
    }
}