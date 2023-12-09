using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces.Repositories;

namespace Tasker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository) { _projectRepository = projectRepository; }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]string id)
        {
            var proj = _projectRepository.Get(id);
            return proj == null ? NotFound() : Ok(JsonSerializer.Serialize(proj));
        }

        [HttpPost]
        public IActionResult Post([FromBody]ProjectDTO dto)
        {
            var created = _projectRepository.Create(dto);

            if (!created) 
            {
                return Conflict(new { error = $"Project with name {dto.Title} already exists" });
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]string id, [FromBody]ProjectDTO dto)
        {
            var updated = _projectRepository.Update(id, dto);

            if (!updated)
            {
                return NotFound(new { error = $"Project with id {id} does not exist" });
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]string id)
        {
            var deleted = _projectRepository.Delete(id);

            if (!deleted)
            {
                return NotFound(new { error = $"Project with id {id} does not exist" });
            }

            return Ok();
        }
    }
}
