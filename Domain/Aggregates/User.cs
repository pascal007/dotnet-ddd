using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Domain.Aggregates;
public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;

    private User() { }

    public User(Guid id, string firstName, string lastName, Email email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name required");

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }
}
