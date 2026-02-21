

using WalletDemo.Domain.Common;

namespace WalletDemo.Domain.Events;


public class CreditFailedEvent : IDomainEvent
{
    public Guid TransferId { get; init; }
    public Guid ToWalletId { get; init; }
    public decimal Amount { get; init; }
    public string Reason { get; init; }

    public Guid AggregateId { get; init; }

    public DateTime OccurredOn { get; init; }

    public CreditFailedEvent()
    {

    }

    public CreditFailedEvent(Guid transferId, Guid toWalletId, decimal amount, string reason)
    {
        TransferId = transferId;
        ToWalletId = toWalletId;
        Amount = amount;
        Reason = reason;
        AggregateId = toWalletId;
        OccurredOn = DateTime.UtcNow;
    }
}
