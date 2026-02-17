using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates;

namespace WalletDemo.Application.Wallets.Commands;

public class CreateWalletHandler : IRequestHandler<CreateWalletCommand, Guid>
{
    private readonly IWalletRepository _repository;

    public CreateWalletHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = new Wallet(Guid.NewGuid(), request.Owner, request.Currency);

        await _repository.AddAsync(wallet);
        await _repository.SaveChangesAsync();

        return wallet.Id;
    }
}
