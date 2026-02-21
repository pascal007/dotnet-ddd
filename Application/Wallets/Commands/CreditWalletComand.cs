
using MediatR;

namespace WalletDemo.Application.Wallets.Commands;

public record CreditWalletComand(Guid TransferId, Guid ToWalletId, decimal Amount) : IRequest;
