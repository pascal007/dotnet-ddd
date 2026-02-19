
using MediatR;

public record DebitWalletCommand(Guid TransferId, Guid WalletId, decimal Amount) : IRequest;
