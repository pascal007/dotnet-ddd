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
    private readonly IUnitOfWork _unitOfWork;

    public CreateWalletHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (!supportedCurrencies.Contains(request.Currency))
            throw new DomainException("Submitted currency not supported");

        bool existingWallet = await _repository.ExistsAsync(request.Currency, request.UserId);

        if (existingWallet)
            throw new DomainException("You already have a " + request.Currency + " wallet");

        var wallet = Wallet.Create(request.UserId, request.Currency);

        _unitOfWork.Track(wallet);

        await _unitOfWork.SaveChangesAsync();

        return wallet.Id;
    }

}
