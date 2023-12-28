using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Queries;
using Tasker.Application.Interfaces.Services;

namespace Tasker.Controllers
{
    [ApiController]
    [Authorize]
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var id = await _userQuery.GetUserId(HttpContext);

            if (id is null)
            {
                return Unauthorized();
            }

            var allowedProjects = (await _service.GetAllAsync()).Where(project =>
                (project.AssignedUsers ?? new List<string>()).Contains(id) ||
                (project.AdminProjects ?? new List<string>()).Contains(id));

            return allowedProjects.Any() ? Ok(allowedProjects) : NoContent();
        }
        
        [HttpGet("project-members/{id}")]
        public async Task<IActionResult> GetProjectMembers([FromRoute] string id)
        {
            var members = await _service.GetMembersAsync(id);
                
            return members.Any() ? Ok(members) : NoContent();
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
                : (dto.AssignedUsers ?? new List<string>()).Contains(userId) ||
                  (dto.AdminProjects ?? new List<string>()).Contains(userId)
                    ? Ok(dto)
                    : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectCreateDto dto)
        {
            dto.UserId = await _userQuery.GetUserId(HttpContext);
            
            var createdDto = await _service.CreateAsync(dto);
            
            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjectUpdateDto dto)
        {
            var updatedDto = await _service.UpdateAsync(dto);

            return Ok(updatedDto);
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