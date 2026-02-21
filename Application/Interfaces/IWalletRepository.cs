using Domain.Aggregates.User;
using Domain.Aggregates.Wallet;

namespace WalletDemo.Application.Interfaces;

public interface IWalletRepository
{

    Task<Wallet?> GetByIdAsync(Guid id);

    Task<bool> ExistsAsync(string currency, Guid owner);



}