
using MediatR;

namespace WalletDemo.Application.Transfers.Commands;

public record TransferCommand(Guid UserId, Guid FromWalletId, Guid ToWalletId, decimal Amount) : IRequest;
