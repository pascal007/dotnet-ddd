using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);

        if (Amount < other.Amount)
            throw new DomainException("Insufficient funds.");

        return new Money(Amount - other.Amount, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Currency mismatch.");
    }
}
