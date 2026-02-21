using Domain.Aggregates.Wallet;
using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Application.Transfers.Commands;
using WalletDemo.Domain.Aggregates.Transfer;
using WalletDemo.Domain.Exceptions;

namespace Application.Transfers.Commands;
public class TransferHandler : IRequestHandler<TransferCommand, Guid>
{
    private readonly ITransferRepository _transferRepository;
    private readonly IWalletReadRepository _walletReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferHandler(ITransferRepository transferRepository, IUnitOfWork unitOfWork, 
        IWalletReadRepository walletReadRepository)
    {
        _transferRepository = transferRepository;
        _unitOfWork = unitOfWork;
        _walletReadRepository = walletReadRepository;
    }

    public async Task<Guid> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var sendWallet = await _walletReadRepository.GetByIdAndOwnerAsync(request.FromWalletId, request.UserId);

        if (sendWallet == null)
        {
            throw new DomainException("Invalid sender wallet");
        }

        if (sendWallet.Value.Balance < request.Amount)
        {
            throw new DomainException("Insufficient balance");
        }

        var receiveWallet = await _walletReadRepository.GetByIdAndCurrencyAsync(request.ToWalletId, sendWallet.Value.Currency);

        if (receiveWallet == null)
        {
            throw new DomainException("Source and target wallet currency differs");
        }

        var transfer = Transfer.Create(
            request.FromWalletId,
            request.ToWalletId,
            request.Amount);

        await _transferRepository.AddAsync(transfer);
        _unitOfWork.Track(transfer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return transfer.Id;
    }
}

