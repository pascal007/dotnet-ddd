

using WalletDemo.Domain.Common;

namespace WalletDemo.Domain.Events;

public class TransferInitiatedEvent : IDomainEvent
{
    public Guid TransferId { get; }
    public Guid FromWalletId { get; }
    public Guid ToWalletId { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TransferInitiatedEvent(Guid transferId, Guid fromWalletId, Guid toWalletId, decimal amount)
    {
        TransferId = transferId;
        FromWalletId = fromWalletId;
        ToWalletId = toWalletId;
        Amount = amount;
    }
}
