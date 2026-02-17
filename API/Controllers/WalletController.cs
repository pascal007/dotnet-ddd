using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletDemo.Application.Common;
using WalletDemo.Application.Wallets.Commands;
using WalletDemo.Application.Wallets.Queries;

namespace WalletDemo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWalletCommand command)
    {
        var walletId = await _mediator.Send(command);
        return Ok(ApiResponse<Guid>.Ok(walletId));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetWalletByIdQuery(id));

        if (result == null)
            return NotFound(ApiResponse<WalletDto>.Fail("Wallet not found"));

        return Ok(ApiResponse<WalletDto>.Ok(result));
    }

    [HttpPost("{id}/debit")]
    public async Task<IActionResult> Debit(Guid id, [FromBody] decimal amount)
    {
        await _mediator.Send(new DebitWalletCommand(id, amount));
        return Ok(ApiResponse<string>.Ok("Debited successfully"));
    }

}
