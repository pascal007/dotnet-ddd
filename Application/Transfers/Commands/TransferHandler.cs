using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Application.Transfers.Commands;
using WalletDemo.Domain.Aggregates.Transfer;

namespace Application.Transfers.Commands;
public class TransferHandler : IRequestHandler<TransferCommand>
{
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferHandler(ITransferRepository transferRepository, IUnitOfWork unitOfWork)
    {
        _transferRepository = transferRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = Transfer.Create(
            request.FromWalletId,
            request.ToWalletId,
            request.Amount);

        await _transferRepository.AddAsync(transfer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
