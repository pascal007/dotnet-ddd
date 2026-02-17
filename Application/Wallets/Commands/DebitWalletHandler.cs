using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Application.Wallets.Commands;

public class DebitWalletHandler : IRequestHandler<DebitWalletCommand>
{
    private readonly IWalletRepository _repository;

    public DebitWalletHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DebitWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _repository.GetByIdAsync(request.WalletId);

        if (wallet == null)
            throw new Exception("Wallet not found.");

        wallet.Debit(new Money(request.Amount, wallet.Balance.Currency));

        await _repository.SaveChangesAsync();
    }
}
