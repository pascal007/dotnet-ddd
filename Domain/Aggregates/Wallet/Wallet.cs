using WalletDemo.Domain.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace Domain.Aggregates.Wallet;

public class Wallet : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid TransferId { get; private set; }
    public Guid Owner { get; private set; } = default!;
    public Money Balance { get; private set; } = default!;


    private Wallet() { } 

    public Wallet(Guid id, Guid owner, string currency)
    {
        if (string.IsNullOrWhiteSpace(owner.ToString()))
            throw new DomainException("Owner cannot be empty.");
            
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");
        Id = id;
        Owner = owner;
        Balance = Money.Zero(currency);
        AddDomainEvent(new WalletCreatedEvent(Id, currency, owner));
        Console.WriteLine($"[DOMAIN] WalletCreatedEvent added for WalletId: {Id}");

    }

    public void Credit(Guid TransferId, Money amount)
    {
        Balance = Balance.Add(amount);
        AddDomainEvent(new WalletCreditedEvent(TransferId, Id, amount.Amount));

    }

    public void Debit(Guid transferId, Money amount)
    {
        if (Balance.Amount < amount.Amount)
            throw new DomainException("Insufficient funds.");

        Balance = Balance.Subtract(amount);
        AddDomainEvent(new WalletDebitedEvent(transferId, Id, amount.Amount));

    }

}
