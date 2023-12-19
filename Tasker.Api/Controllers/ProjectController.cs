using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Queries;
using Tasker.Application.Interfaces.Services;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IGetUserQuery _userQuery;

        public ProjectController(IProjectService service, IGetUserQuery userQuery)
        {
            _service = service;
            _userQuery = userQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var id = await _userQuery.GetUserId(HttpContext);

            if (id is null)
            {
                return Unauthorized();
            }

            var allowedProjects = (await _service.GetAllAsync()).Where(project =>
                (project.AssignedProjects ?? new List<string>()).Contains(id) ||
                (project.UnderControlProjects ?? new List<string>()).Contains(id));

            return allowedProjects.Any() ? Ok(allowedProjects) : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var userId = await _userQuery.GetUserId(HttpContext);

            if (userId is null)
            {
                return Unauthorized();
            }

            var dto = await _service.GetByIdAsync(id);
            
            return dto is null
                ? NotFound()
                : (dto.AssignedProjects ?? new List<string>()).Contains(userId) ||
                  (dto.UnderControlProjects ?? new List<string>()).Contains(userId)
                    ? Ok(dto)
                    : NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectCreateDto dto)
        {
            var createdDto = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjectUpdateDto dto)
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
                : NotFound(new { error = $"Project with id {id} does not exist" });
        }
    }
}