using MediatR;

namespace WalletDemo.Application.Wallets.Commands;

public record CreateWalletCommand( Guid UserId, string Currency) : IRequest<Guid>;
