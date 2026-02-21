

using MediatR;
using WalletDemo.Application.Common;
using WalletDemo.Application.Interfaces;
using WalletDemo.Application.Wallets.Commands;
using WalletDemo.Domain.Aggregates.Transfer;
using WalletDemo.Domain.Events;

namespace WalletDemo.Application.Transfers.Commands;
public class TransferSaga :
    INotificationHandler<DomainEventNotification<TransferInitiatedEvent>>,
    INotificationHandler<DomainEventNotification<WalletDebitedEvent>>,
    INotificationHandler<DomainEventNotification<WalletCreditedEvent>>,
    INotificationHandler<DomainEventNotification<CreditFailedEvent>>,
    INotificationHandler<DomainEventNotification<WalletRefundedEvent>>

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
            transfer.Id,
            transfer.ToWalletId,
            transfer.Amount));
    }

    public async Task Handle(DomainEventNotification<WalletCreditedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;


        if (e.TransferId != Guid.Empty)
        {
            var transfer = await _transferRepository.GetByIdAsync(e.TransferId);
            if (transfer != null) {
                transfer.MarkCompleted();
            }

        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"[SAGA] Credit completed {e.TransferId}");
    }

    public async Task Handle(DomainEventNotification<CreditFailedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        Console.WriteLine($"[SAGA] Credit failed for {e.TransferId}");

        if (e.TransferId == Guid.Empty)
        {
            Console.WriteLine("[SAGA] CreditFailedEvent without TransferId ignored.");
            return;
        }

        var transfer = await _transferRepository.GetByIdAsync(e.TransferId);

        if (transfer == null)
        {
            return;
        }

        if (transfer.Status != TransferStatus.Debited)
            return;

        await _mediator.Send(new RefundWalletCommand(
            e.TransferId,
            transfer.FromWalletId,
            e.Amount));
    }

    public async Task Handle(DomainEventNotification<WalletRefundedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        var transfer = await _transferRepository.GetByIdAsync(e.TransferId);

        transfer.MarkFailed();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"[SAGA] Transfer {e.TransferId} refunded and marked failed");
    }


}
