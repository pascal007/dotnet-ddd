using WalletDemo.Domain.Common;

public class WalletCreditedEvent : IDomainEvent
{
    public Guid WalletId { get; }
    public decimal Amount { get; }
    public Guid Owner { get; }
    public Guid TransferId { get; }

    public string Currency { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WalletCreditedEvent(Guid transferId, Guid walletId, decimal amount)
    {
        WalletId = walletId;
        Amount = amount;
        TransferId = transferId;
    }
}