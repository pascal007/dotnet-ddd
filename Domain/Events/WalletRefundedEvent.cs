

using WalletDemo.Domain.Common;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Domain.Events;

public class WalletRefundedEvent : IDomainEvent
{
    public Guid WalletId { get; init; }
    public decimal Amount { get; init; }
    public Guid TransferId { get; init; }

    public string Currency { get; init; }

    public DateTime OccurredOn { get; init; }

    public Guid AggregateId { get; init; }

    public WalletRefundedEvent()
    {

    }

    public WalletRefundedEvent(Guid walletId, Guid transferId, Money money)
    {
        WalletId = walletId;
        TransferId = transferId;
        Amount = money.Amount;
        Currency = money.Currency;
        AggregateId = walletId; 
        OccurredOn = DateTime.UtcNow;
    }
}
