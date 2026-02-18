using MediatR;

namespace WalletDemo.Application.Wallets.Queries;

public record GetWalletByIdQuery(Guid Id, Guid UserId) : IRequest<WalletDto?>;
