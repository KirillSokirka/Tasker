using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        public IActionResult Project([FromRoute]string id)
        {
            var proj = _projectRepository.Get(id);
            return proj == null ? NotFound() : Ok(JsonSerializer.Serialize(proj));
        }

        [HttpPost("{title}")]
        public IActionResult Post([FromRoute]string title)
        {
            var created = _projectRepository.Create(title);

            if (!created) 
            {
                return Conflict(new { error = $"Project with name {title} already exists" });
            }

            return Ok();
        }

        [HttpPut("{id}/{title}")]
        public IActionResult Put([FromRoute] string id, [FromRoute] string title)
        {
            var updated = _projectRepository.Update(id, title);

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
