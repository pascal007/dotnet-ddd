using System.Text.RegularExpressions;
using WalletDemo.Domain.Exceptions;

namespace WalletDemo.Domain.ValueObjects;

public sealed class Email
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (!EmailRegex.IsMatch(value))
            throw new DomainException("Invalid email format.");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;

    private Email() { Value = default!; }
}
