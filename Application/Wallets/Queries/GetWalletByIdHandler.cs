using MediatR;
using WalletDemo.Application.Interfaces;

namespace WalletDemo.Application.Wallets.Queries;

public class GetWalletByIdHandler : IRequestHandler<GetWalletByIdQuery, WalletDto?>
{
    private readonly IWalletRepository _repository;

    public GetWalletByIdHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<WalletDto?> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _repository.GetByIdAndOwnerAsync(request.Id, request.UserId.ToString());

        if (wallet == null)
            return null;

        return new WalletDto
        {
            Id = wallet.Id,
            Balance = wallet.Balance.Amount,
            Currency = wallet.Balance.Currency
        };
    }
}
