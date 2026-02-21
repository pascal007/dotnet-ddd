using WalletDemo.Domain.Common;

namespace WalletDemo.Domain.Events;

public class WalletCreatedEvent : IDomainEvent
{
    public Guid AggregateId { get; init; }

    public Guid WalletId { get; init; }
    public string Currency { get; init; }

    public Guid Owner { get; init; }

    public DateTime OccurredOn { get; init; }

    public WalletCreatedEvent() { 
    
    }

    public WalletCreatedEvent(Guid walletId, string currency, Guid owner)
    {
        WalletId = walletId;
        Currency = currency;
        Owner = owner;
        AggregateId = walletId;
        OccurredOn = DateTime.Now;
    }
}
