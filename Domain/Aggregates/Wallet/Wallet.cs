using WalletDemo.Domain.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace Domain.Aggregates.Wallet;

public class Wallet : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid Owner { get; private set; }
    public Money Balance { get; private set; } = default!;

    private Wallet() { } 

    public static Wallet Create(Guid owner, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        var wallet = new Wallet();

        var @event = new WalletCreatedEvent(Guid.NewGuid(), currency, owner);

        wallet.Apply(@event);
        wallet.AddDomainEvent(@event);

        return wallet;
    }

    public void Credit(Guid transferId, Money money)
    {
        var @event = new WalletCreditedEvent(Id, money, Owner, transferId);

        Apply(@event);
        AddDomainEvent(@event);
    }

    public void Debit(Guid transferId, Money amount)
    {
        if (Balance.Amount < amount.Amount)
            throw new DomainException("Insufficient funds.");

        var @event = new WalletDebitedEvent(transferId, Id, amount, Owner);

        Apply(@event);
        AddDomainEvent(@event);
    }

    private void Apply(WalletCreatedEvent e)
    {
        Id = e.WalletId;
        Owner = e.Owner;
        Balance = Money.Zero(e.Currency);
    }

    private void Apply(WalletCreditedEvent e)
    {
        Balance = Balance.Add(new Money(e.Amount, Balance.Currency));
    }

    private void Apply(WalletDebitedEvent e)
    {
        Balance = Balance.Subtract(new Money(e.Amount, Balance.Currency));
    }
    private void Apply(WalletRefundedEvent e)
    {
        Console.WriteLine("refunding " + e.Amount);
        Balance = Balance.Add(new Money(e.Amount, Balance.Currency));
    }

    public static Wallet Rehydrate(IEnumerable<IDomainEvent> events)
    {
        var wallet = new Wallet();

        foreach (var @event in events.OrderBy(e => e.OccurredOn))
        {
            switch (@event)
            {
                case WalletCreatedEvent created:
                    wallet.Apply(created);
                    break;

                case WalletCreditedEvent credited:
                    wallet.Apply(credited);
                    break;

                case WalletDebitedEvent debited:
                    wallet.Apply(debited);
                    break;

                case WalletRefundedEvent refunded:
                    wallet.Apply(refunded);
                    break;
            }
        }

        return wallet;
    }

    public void Refund(decimal amount, Guid transferId)
    {
        Money money = new Money(amount, Balance.Currency);
        var @event = new WalletRefundedEvent(Id, transferId, money);
        Apply(@event);
        AddDomainEvent(new WalletRefundedEvent(Id, transferId, money));
    }

}
