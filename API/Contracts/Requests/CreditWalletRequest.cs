using MediatR;

namespace WalletDemo.API.Contracts.Requests;

public record CreditWalletRequest(decimal Amount) : IRequest;
