using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Domain.Aggregates;

public class Wallet
{
    public Guid Id { get; private set; }
    public string Owner { get; private set; } = default!;
    public Money Balance { get; private set; } = default!;


    private Wallet() { } 

    public Wallet(Guid id, string owner, string currency)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new DomainException("Owner cannot be empty.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");
        Id = id;
        Owner = owner;
        Balance = Money.Zero(currency);
    }

    public void Credit(Money amount)
    {
        Balance = Balance.Add(amount);
    }

    public void Debit(Money amount)
    {
        if (amount.Amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        if (Balance.Amount < amount.Amount)
            throw new DomainException("Insufficient funds.");

        Balance = Balance.Subtract(amount);
    }

}
