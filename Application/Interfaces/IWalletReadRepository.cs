
namespace WalletDemo.Application.Interfaces;
public interface IWalletReadRepository
{
    Task<decimal?> GetBalanceAsync(Guid walletId, Guid UserId);
}
