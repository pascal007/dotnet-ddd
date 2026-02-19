using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletDemo.Application.Common;
using WalletDemo.Application.Users.Commands;

namespace WalletDemo.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(ApiResponse<Guid>.Ok(userId));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(ApiResponse<string>.Ok(token));
    }

}
