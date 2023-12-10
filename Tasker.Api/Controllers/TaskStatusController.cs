using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces.Repositories;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/taskStatus")]
    public class TaskStatusController : ControllerBase
    {
        private readonly ITaskStatusRepository _statusRepository;

        public TaskStatusController(ITaskStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var dto = await _statusRepository.GetAsync(id);

            return dto is null
                ? NotFound()
                : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskStatusDto dto)
        {
            var createdDto = await _statusRepository.CreateAsync(dto);

            return createdDto is null
                ? Conflict(new { error = $"TaskStatus with name {dto.Name} already exists" })
                : CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskStatusDto dto)
        {
            var updatedDto = await _statusRepository.UpdateAsync(dto);

            return updatedDto is null
                ? NotFound(new { error = $"TaskStatus with id {dto.Id} does not exist" })
                : Ok(updatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var deleted = await _statusRepository.DeleteAsync(id);

            return deleted
                ? NoContent()
                : NotFound(new { error = $"TaskStatus with id {id} does not exist" });
        }
    }
}