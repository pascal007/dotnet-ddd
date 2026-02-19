using Domain.Aggregates.Wallet;

namespace WalletDemo.Application.Interfaces;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet);
    Task<Wallet?> GetByIdAsync(Guid id);
    Task<Wallet?> GetByIdAndOwnerAsync(Guid id, Guid owner);
    Task<List<Wallet>?> GetByOwnerAsync(Guid owner);

    Task<Wallet?> GetByCurrencyAndOwnerAsync(string currency, Guid owner);

    public Task<Wallet?> GetByCurrencyAndOwnerWithLockAsync(string currency, Guid owner);
    Task SaveChangesAsync();
}
