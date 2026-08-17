using BackendChallange.Api.DTOs;
using BackendChallange.Api.Models;
using BackendChallange.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendChallange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get()
    {
        var users = _userService.GetAll();

        if (users.Count == 0)
        {
            return NotFound(new
            {
                message = "Nenhum usuário cadastrado."
            });
        }

        var response = users
            .Select(MapToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var user = _userService.GetById(id);

            return Ok(MapToResponse(user));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Create(CreateUserRequest request)
    {
        try
        {
            var user = _userService.Create(request);

            return StatusCode(
                StatusCodes.Status201Created,
                MapToResponse(user)
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Update(Guid id, UpdateUserRequest request)
    {
        try
        {
            var user = _userService.Update(id, request);

            return Ok(new
            {
                message = "Usuário atualizado com sucesso.",
                user = MapToResponse(user)
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _userService.Delete(id);

            return Ok(new
            {
                message = "Usuário excluído com sucesso."
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
            BirthDate = user.BirthDate
        };
    }
}