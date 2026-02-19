using Domain.Aggregates.Wallet;
using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates;
using WalletDemo.Domain.Common;
using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Application.Wallets.Commands;

public class CreateWalletHandler : IRequestHandler<CreateWalletCommand, Guid>
{
    private readonly IWalletRepository _repository;
    private readonly List<String> supportedCurrencies = new List<String>() { Currencies.GBP, Currencies.USD};

    public CreateWalletHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (!supportedCurrencies.Contains(request.Currency))
        {
            throw new DomainException("Submitted currency not supported");
        }
        var existingWallet = await _repository.GetByCurrencyAndOwnerAsync(request.Currency, request.UserId);
        if (existingWallet != null) {
            throw new DomainException("You already have a " + request.Currency + " wallet");
        }
        var wallet = new Wallet(Guid.NewGuid(), request.UserId, request.Currency);

        await _repository.AddAsync(wallet);
        await _repository.SaveChangesAsync();

        return wallet.Id;
    }
}
