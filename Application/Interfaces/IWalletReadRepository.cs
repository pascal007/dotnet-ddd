
namespace WalletDemo.Application.Interfaces;

public interface IWalletReadRepository
{
    Task<decimal?> GetBalanceAsync(Guid walletId, Guid userId);

    Task<bool> ExistsAsync(string currency, Guid ownerId);


    Task<List<(Guid Id, decimal Balance, string Currency)>> GetByOwnerAsync(Guid ownerId);

    Task<(Guid Id, decimal Balance, string Currency)?>GetByIdAndOwnerAsync(Guid walletId, Guid ownerId);
    Task<(Guid Id, decimal Balance, string Currency)?>GetByIdAndCurrencyAsync(Guid walletId, string currency);


}