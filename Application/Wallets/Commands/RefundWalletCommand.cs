

using MediatR;

namespace WalletDemo.Application.Wallets.Commands;

public record RefundWalletCommand(Guid TransferId, Guid WalletId, decimal Amount) : IRequest;
