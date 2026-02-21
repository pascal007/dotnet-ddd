

using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Application.Wallets.Queries;
using WalletDemo.Domain.Aggregates;

namespace Application.Wallets.Queries;

public class GetAllWalletsHandler : IRequestHandler<GetAllWalletsQuery, List<WalletDto>>
{
    private readonly IWalletReadRepository _readRepository;

    public GetAllWalletsHandler(IWalletReadRepository repository)
    {
        _readRepository = repository;
    }

    public async Task<List<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var wallets = await _readRepository.GetByOwnerAsync(request.UserId);

        return wallets.Select(w => new WalletDto
        {
            Id = w.Id,
            Balance = w.Balance,
            Currency = w.Currency
        }).ToList();
    }

}
