
using MediatR;

namespace WalletDemo.Application.Wallets.Queries;

public record GetAllWalletsQuery(Guid UserId) : IRequest<List<WalletDto>>;
