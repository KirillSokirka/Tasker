using Microsoft.AspNetCore.Mvc;
using Tasker.Application.Interfaces.Repositories;

namespace Tasker.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var dto = await _userRepository.GetAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _userRepository.GetAllAsync());

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deleted = await _userRepository.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new { error = $"Project with id {id} does not exist" });
    }
}