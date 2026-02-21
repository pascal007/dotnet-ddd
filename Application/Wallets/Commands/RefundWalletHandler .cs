using MediatR;

using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Application.Wallets.Commands;

public class RefundWalletHandler : IRequestHandler<RefundWalletCommand>
{
    private readonly IWalletRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RefundWalletHandler(IWalletRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RefundWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _repository.GetByIdAsync(request.WalletId);

        if (wallet == null)
            throw new DomainException("Wallet not found for refund");

        wallet.Refund(request.Amount, request.TransferId);

        _unitOfWork.Track(wallet); 

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
