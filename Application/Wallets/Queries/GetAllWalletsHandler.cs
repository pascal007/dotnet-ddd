

using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Application.Wallets.Queries;
using WalletDemo.Domain.Aggregates;

namespace Application.Wallets.Queries;



public class GetAllWalletsHandler : IRequestHandler<GetAllWalletsQuery, List<WalletDto>>
{
    private readonly IWalletRepository _repository;

    public GetAllWalletsHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var wallets = await _repository.GetByOwnerAsync(request.UserId.ToString());

        return wallets.Select(w => new WalletDto
        {
            Id = w.Id,
            Balance = w.Balance.Amount,
            Currency = w.Balance.Currency
        }).ToList();
    }
}
