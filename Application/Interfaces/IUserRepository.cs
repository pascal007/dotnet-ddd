
using WalletDemo.Domain.Aggregates;
using WalletDemo.Domain.ValueObjects;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(Email email);
}
