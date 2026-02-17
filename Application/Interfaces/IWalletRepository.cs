using WalletDemo.Domain.Aggregates;

namespace WalletDemo.Application.Interfaces;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet);
    Task<Wallet?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
