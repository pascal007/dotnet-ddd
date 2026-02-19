

using Application.Wallets.Commands;
using MediatR;
using WalletDemo.Application.Common;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Events;

namespace WalletDemo.Application.Transfers.Commands;
public class TransferSaga :
    INotificationHandler<DomainEventNotification<TransferInitiatedEvent>>,
    INotificationHandler<DomainEventNotification<WalletDebitedEvent>>,
    INotificationHandler<DomainEventNotification<WalletCreditedEvent>>
{
    private readonly IMediator _mediator;
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferSaga(IMediator mediator, ITransferRepository transferRepository, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _transferRepository = transferRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DomainEventNotification<TransferInitiatedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        Console.WriteLine($"[SAGA] Transfer initiated {e.TransferId}");

        await _mediator.Send(new DebitWalletCommand(
            e.TransferId,
            e.FromWalletId,
            e.Amount));
    }

    public async Task Handle(DomainEventNotification<WalletDebitedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        var transfer = await _transferRepository.GetByIdAsync(e.TransferId);

        if (transfer == null) return;

        transfer.MarkDebited();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new CreditWalletComand(
            e.TransferId,
            transfer.ToWalletId,
            transfer.Amount));
    }

    public async Task Handle(DomainEventNotification<WalletCreditedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        var transfer = await _transferRepository.GetByIdAsync(e.TransferId);

        if (transfer == null) return;

        transfer.MarkCompleted();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"[SAGA] Transfer completed {e.TransferId}");
    }
}
