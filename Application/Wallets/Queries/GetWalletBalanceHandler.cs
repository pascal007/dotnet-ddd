using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Application.Wallets;

public class GetWalletBalanceHandler : IRequestHandler<GetWalletBalanceQuery, decimal>
{
    private readonly IWalletReadRepository _readRepository;

    public GetWalletBalanceHandler(IWalletReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<decimal> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _readRepository.GetBalanceAsync(request.WalletId, request.UserId);

        if (wallet == null)
            throw new DomainException("Wallet not found");
        return wallet.Value;
    }
}
