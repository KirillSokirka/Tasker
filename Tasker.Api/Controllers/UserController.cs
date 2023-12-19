using Microsoft.AspNetCore.Mvc;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Queries;

namespace Tasker.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IGetUserQuery _userQuery;

    public UserController(IUserService service, IGetUserQuery userQuery)
    {
        _service = service;
        _userQuery = userQuery;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var dto = await _service.GetByIdAsync(id);

        return dto is null
            ? NotFound()
            : Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
    {
        var updatedDto = await _service.UpdateAsync(dto);
        
        return updatedDto is null
            ? NotFound()
            : Ok(updatedDto);
    } 
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if(id == "-1")
        {
            id = await _userQuery.GetUserId(HttpContext);
        }
        
        var deleted = await _service.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new { error = $"User with id {id} does not exist" });
    }
}