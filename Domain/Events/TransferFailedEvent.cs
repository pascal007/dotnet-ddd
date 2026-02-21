

using WalletDemo.Domain.Common;

namespace WalletDemo.Domain.Events;

public class TransferFailedEvent : IDomainEvent
{
    public Guid TransferId { get; init; }
    public string Reason { get; init; }

    public Guid AggregateId { get; init; }

    public DateTime OccurredOn { get; init; }

    public TransferFailedEvent() { }

    public TransferFailedEvent(Guid transferId, string reason, Guid aggregateId)
    {
        TransferId = transferId;
        Reason = reason;
        AggregateId = aggregateId;
        OccurredOn = DateTime.Now;
    }
}
