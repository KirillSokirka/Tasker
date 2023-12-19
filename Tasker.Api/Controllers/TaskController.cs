using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Interfaces.Services;

namespace Tasker.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllByProject([FromRoute] string projectId)
    {
        var allTasks = await _service.GetAllAsync();
        
        return Ok(allTasks.Where(t => t.ProjectId == projectId));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var dto = await _service.GetByIdAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TaskCreateDto dto)
    {
        var createdDto = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TaskUpdateDto dto)
    {
        var updatedDto = await _service.UpdateAsync(dto);

        return updatedDto is null
            ? NotFound(new { error = $"Task with id {dto.Id} does not exist" })
            : Ok(updatedDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deleted = await _service.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new { error = $"Task with id {id} does not exist" });
    }
}