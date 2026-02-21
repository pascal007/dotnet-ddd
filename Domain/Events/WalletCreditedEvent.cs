using WalletDemo.Domain.Common;
using WalletDemo.Domain.ValueObjects;

public class WalletCreditedEvent : IDomainEvent
{
    public Guid WalletId { get; init; }
    public decimal Amount { get; init; }
    public Guid Owner { get; init; }
    public Guid TransferId { get; init; }

    public string Currency { get; init; }

    public DateTime OccurredOn { get; init; }

    public Guid AggregateId { get; init; }

    public WalletCreditedEvent() { }

    public WalletCreditedEvent(Guid aggregateId, Money money, Guid owner, Guid transferId)
    {
        WalletId = aggregateId;
        Amount = money.Amount;
        Currency = money.Currency;
        AggregateId = aggregateId;
        Owner = owner;
        TransferId = transferId;
        OccurredOn = DateTime.Now;
    }
}