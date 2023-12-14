using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.Interfaces.Repositories;

namespace Tasker.Controllers
{
    [ApiController]
    [Route("api/kanbanBoards")]
    public class KanbanBoardController : ControllerBase
    {
        private readonly IKanbanBoardRepository _boardRepository;

        public KanbanBoardController(IKanbanBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        => Ok(await _boardRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var dto = await _boardRepository.GetAsync(id);

            return dto is null
                ? NotFound()
                : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KanbanBoardCreateDto dto)
        {
            var createdDto = await _boardRepository.CreateAsync(dto);

            return createdDto is null
                ? Conflict(new { error = $"KanbanBoard with name {dto.Title} already exists" })
                : CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] KanbanBoardUpdateDto dto)
        {
            var updatedDto = await _boardRepository.UpdateAsync(dto);

            return updatedDto is null
                ? NotFound(new { error = $"KanbanBoard with id {dto.Id} does not exist" })
                : Ok(updatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var deleted = await _boardRepository.DeleteAsync(id);

            return deleted
                ? NoContent()
                : NotFound(new { error = $"KanbanBoard with id {id} does not exist" });
        }
    }
}