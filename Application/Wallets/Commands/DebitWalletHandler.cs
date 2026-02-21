using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Application.Wallets.Commands;

public class DebitWalletHandler : IRequestHandler<DebitWalletCommand>
{
    private readonly IWalletRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DebitWalletHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DebitWalletCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        var wallet = await _repository.GetByIdAsync(request.WalletId);

        if (wallet == null)
            throw new DomainException("Wallet not found.");

        wallet.Debit(request.TransferId, new Money(request.Amount, wallet.Balance.Currency));
        _unitOfWork.Track(wallet);
        await _unitOfWork.SaveChangesAsync();

    }
}
