
using MediatR;

public record DebitWalletCommand(Guid WalletId, decimal Amount) : IRequest;
