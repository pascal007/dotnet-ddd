
using MediatR;
using WalletDemo.Application.Common;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates.Transfer;
using WalletDemo.Domain.Events;
using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Application.Wallets.Commands;

public class CreditWalletHandler : IRequestHandler<CreditWalletComand>
{
    private readonly IWalletRepository _repository;
    private readonly IWalletReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ITransferRepository _transferRepository;

    public CreditWalletHandler(
        IWalletRepository repository, IUnitOfWork unitOfWork, IWalletReadRepository readRepository, IMediator mediator,
        ITransferRepository transferRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _readRepository = readRepository;
        _mediator = mediator;
        _transferRepository = transferRepository;
    }

    public async Task Handle(CreditWalletComand request, CancellationToken cancellationToken)
    {

        if (request.Amount <= 0)
            throw new DomainException("Amount must be greater than zero.");


        var wallet = await _repository.GetByIdAsync(request.ToWalletId);


        if (wallet == null)
            throw new DomainException("Wallet not found.");
        Console.WriteLine($"Loaded wallet Id: {wallet.Id}");

        try
        {
            throw new DomainException("Transfer failed");
            wallet.Credit(request.TransferId, new Money(request.Amount, wallet.Balance.Currency));
            _unitOfWork.Track(wallet);
            await _unitOfWork.SaveChangesAsync();

        }
        catch (Exception ex)
        {

            await _mediator.Publish(
                new DomainEventNotification<CreditFailedEvent>(new CreditFailedEvent(
                        request.TransferId, request.ToWalletId, request.Amount, ex.Message)),
                    cancellationToken);
        }

    }
}

