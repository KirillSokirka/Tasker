using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Repositories;

namespace Tasker.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _taskRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var dto = await _taskRepository.GetAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TaskDto dto)
    {
        var createdDto = await _taskRepository.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TaskUpdateDto dto)
    {
        var updatedDto = await _taskRepository.UpdateAsync( dto);

        return updatedDto is null
            ? NotFound(new { error = $"Task with id {dto.Id} does not exist" })
            : Ok(updatedDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deleted = await _taskRepository.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new { error = $"Task with id {id} does not exist" });
    }
}