using WalletDemo.Domain.Common;
using WalletDemo.Domain.ValueObjects;

public class WalletDebitedEvent : IDomainEvent
{
    public Guid WalletId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public Guid TransferId { get; init; }

    public Guid Owner { get; init; }
    public DateTime OccurredOn { get; init; }

    public Guid AggregateId { get; init; }

    public WalletDebitedEvent () { }

    public WalletDebitedEvent(Guid transferId, Guid walletId, Money money, Guid owner)
    {
        WalletId = walletId;
        Amount = money.Amount;
        Currency = money.Currency;
        TransferId = transferId;
        AggregateId = walletId;
        Owner = owner;
        OccurredOn = DateTime.Now;

    }
}
