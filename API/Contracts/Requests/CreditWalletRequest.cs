using MediatR;

namespace WalletDemo.API.Contracts.Requests;

public record CreditWalletRequest(string Currency, decimal Amount) : IRequest;
