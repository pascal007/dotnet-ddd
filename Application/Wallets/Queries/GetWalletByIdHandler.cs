using MediatR;
using WalletDemo.Application.Interfaces;

namespace WalletDemo.Application.Wallets.Queries;

public class GetWalletByIdHandler : IRequestHandler<GetWalletByIdQuery, WalletDto?>
{
    private readonly IWalletReadRepository _readRepository;

    public GetWalletByIdHandler(IWalletReadRepository readRepository)
    {
        _readRepository = readRepository;
    }


    public async Task<WalletDto?> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _readRepository.GetByIdAndOwnerAsync(request.Id, request.UserId);

        if (wallet == null)
            return null;

        return new WalletDto
        {
            Id = wallet.Value.Id,
            Balance = wallet.Value.Balance,
            Currency = wallet.Value.Currency
        };
    }
}
