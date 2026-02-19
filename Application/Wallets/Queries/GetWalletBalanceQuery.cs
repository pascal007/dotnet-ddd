using MediatR;

public record GetWalletBalanceQuery(Guid WalletId, Guid UserId) : IRequest<decimal>;
