using WalletDemo.Domain.Common;

public class WalletDebitedEvent : IDomainEvent
{
    public Guid WalletId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public Guid TransferId { get; }

    public Guid Owner {  get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WalletDebitedEvent(Guid transferId, Guid walletId, decimal amount)
    {
        WalletId = walletId;
        Amount = amount;
        TransferId = transferId;
    }
}
