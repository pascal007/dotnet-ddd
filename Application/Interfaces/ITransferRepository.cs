
using WalletDemo.Domain.Aggregates.Transfer;

namespace WalletDemo.Application.Interfaces;

public interface ITransferRepository
{
    Task AddAsync(Transfer transfer);
    Task<Transfer?> GetByIdAsync(Guid id);
}
