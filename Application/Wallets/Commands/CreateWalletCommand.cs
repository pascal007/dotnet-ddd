using MediatR;

namespace WalletDemo.Application.Wallets.Commands;

public record CreateWalletCommand(string Owner, string Currency) : IRequest<Guid>;
