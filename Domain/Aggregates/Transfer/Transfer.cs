
using WalletDemo.Domain.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Domain.Aggregates.Transfer;

public class Transfer : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid FromWalletId { get; private set; }
    public Guid ToWalletId { get; private set; }
    public decimal Amount { get; private set; }
    public TransferStatus Status { get; private set; }

    private Transfer() { } 

    private Transfer(Guid fromWalletId, Guid toWalletId, decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Transfer amount must be greater than zero.");

        Id = Guid.NewGuid();
        FromWalletId = fromWalletId;
        ToWalletId = toWalletId;
        Amount = amount;
        Status = TransferStatus.Initiated;

        AddDomainEvent(new TransferInitiatedEvent(
            Id,
            FromWalletId,
            ToWalletId,
            Amount));
    }

    public static Transfer Create(Guid fromWalletId, Guid toWalletId, decimal amount) => new Transfer(fromWalletId, toWalletId, amount);

    public void MarkDebited()
    {
        Status = TransferStatus.Debited;
    }

    public void MarkCompleted()
    {
        Status = TransferStatus.Completed;
    }

    public void MarkFailed()
    {
        Status = TransferStatus.Failed;
    }
}
