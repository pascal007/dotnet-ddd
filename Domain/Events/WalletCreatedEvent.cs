using WalletDemo.Domain.Common;

namespace WalletDemo.Domain.Events;

public class WalletCreatedEvent : IDomainEvent
{
    public Guid WalletId { get; }
    public string Currency { get; }

    public Guid Owner { get; }

    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WalletCreatedEvent(Guid walletId, string currency, Guid owner)
    {
        WalletId = walletId;
        Currency = currency;
        Owner = owner;
    }
}
