using Application.Wallets.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletDemo.Api.Contracts.Wallets;
using WalletDemo.API.Common.Extensions;
using WalletDemo.API.Contracts.Requests;
using WalletDemo.Application.Common;
using WalletDemo.Application.Transfers.Commands;
using WalletDemo.Application.Wallets.Commands;
using WalletDemo.Application.Wallets.Queries;

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
    public async Task<IActionResult> CreateWallet(CreateWalletRequest createWalletRequest)
    {
        var userId = User.GetUserId();

        var walletId = await _mediator.Send(new CreateWalletCommand(userId, createWalletRequest.Currency));
        return Ok(ApiResponse<Guid>.Ok(walletId));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetWallet(Guid id)
    {
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetWalletByIdQuery(id, userId));

        if (result == null)
            return NotFound(ApiResponse<WalletDto>.Fail("Wallet not found"));

        return Ok(ApiResponse<WalletDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetWallets()
    {
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetAllWalletsQuery(userId));

        return Ok(ApiResponse<List<WalletDto>>.Ok(result));
    }


    [HttpPost("{id}/credit")]
    public async Task<IActionResult> Credit(Guid id, CreditWalletRequest creditWalletRequest)
    {
        var userId = User.GetUserId();

        await _mediator.Send(new CreditWalletComand(null, id, creditWalletRequest.Amount));
        return Ok(ApiResponse<string>.Ok("Wallet credited successfully"));
    }

    [HttpGet("supportedCurrencies")]
    public async Task<IActionResult> GetCurrencies()
    {
        var result = await _mediator.Send(new GetSupportedCurrenciesQuery());
        return Ok(ApiResponse<List<string>>.Ok(result));
    }

    [HttpGet("walletBalance")]
    public async Task<IActionResult> GetBalance(GetWalletBalanceRequest getWalletBalanceRequest)
    {
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetWalletBalanceQuery(userId, getWalletBalanceRequest.WalletId));
        return Ok(ApiResponse<List<string>>.Ok(null));
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferFundRequest transferFundRequest)
    {
        var userId = User.GetUserId();

        await _mediator.Send(new TransferCommand(userId, transferFundRequest.sourceWallet, transferFundRequest.DestinationWallet,
            transferFundRequest.Amount));
        return Ok(ApiResponse<List<string>>.Ok(null));
    }

}
