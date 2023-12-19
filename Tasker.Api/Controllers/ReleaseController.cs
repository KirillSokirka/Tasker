using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.Interfaces.Services;

namespace Tasker.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/releases")]
    public class ReleaseController : ControllerBase
    {
        private readonly IReleaseService _service;

        public ReleaseController(IReleaseService service)
        {
            _service = service;
        }
        
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("available/{projectId}")]
        public async Task<IActionResult> GetAllByProject(string projectId)
        {
            var allReleases = await _service.GetAllAsync();
        
            return Ok(allReleases.Where(t => t.ProjectId == projectId));
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
        public async Task<IActionResult> Post([FromBody] ReleaseCreateDto dto)
        {
            var createdDto = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ReleaseUpdateDto dto)
        {
            var updatedDto = await _service.UpdateAsync(dto);

            return updatedDto is null
                ? NotFound(new { error = $"Project with id {dto.Id} does not exist" })
                : Ok(updatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var deleted = await _service.DeleteAsync(id);

            return deleted
                ? NoContent()
                : NotFound(new { error = $"Project with id {id} does not exist" });
        }
    }
}