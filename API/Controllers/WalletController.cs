using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletDemo.Api.Contracts.Wallets;
using WalletDemo.API.Common.Extensions;
using WalletDemo.Application.Common;
using WalletDemo.Application.Wallets.Commands;
using WalletDemo.Application.Wallets.Queries;
using WalletDemo.Domain.Aggregates;

namespace WalletDemo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWalletRequest createWalletRequest)
    {
        var userId = User.GetUserId();

        var walletId = await _mediator.Send(new CreateWalletCommand(userId, createWalletRequest.Currency));
        return Ok(ApiResponse<Guid>.Ok(walletId));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetWalletByIdQuery(id, userId));

        if (result == null)
            return NotFound(ApiResponse<WalletDto>.Fail("Wallet not found"));

        return Ok(ApiResponse<WalletDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetAllWalletsQuery(userId));

        return Ok(ApiResponse<List<WalletDto>>.Ok(result));
    }

    [HttpPost("{id}/debit")]
    public async Task<IActionResult> Debit(Guid id, [FromBody] decimal amount)
    {
        await _mediator.Send(new DebitWalletCommand(id, amount));
        return Ok(ApiResponse<string>.Ok("Debited successfully"));
    }

}
