using WalletDemo.Domain.Aggregates;

namespace WalletDemo.Application.Interfaces;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet);
    Task<Wallet?> GetByIdAsync(Guid id);
    Task<Wallet?> GetByIdAndOwnerAsync(Guid id, string owner);
    Task<List<Wallet>?> GetByOwnerAsync(string owner);

    Task<Wallet?> GetByCurrencyAndOwner(string currency, string owner);
    Task SaveChangesAsync();
}
