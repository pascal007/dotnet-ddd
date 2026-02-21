using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Interfaces;
using WalletDemo.Infrastructure.Persistence;

namespace WalletDemo.Infrastructure.Repositories;

public class WalletReadRepository : IWalletReadRepository
{
    private readonly AppDbContext _context;

    public WalletReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> GetBalanceAsync(Guid walletId, Guid userId)
    {
        var readModel = await _context.WalletReadModels
            .FirstOrDefaultAsync(x => x.WalletId == walletId && x.OwnerId == userId);

        return readModel?.CurrentBalance;
    }

    public async Task<bool> ExistsAsync(string currency, Guid ownerId)
    {
        return await _context.WalletReadModels
            .AnyAsync(x => x.Currency == currency && x.OwnerId == ownerId);
    }

    public async Task<List<(Guid Id, decimal Balance, string Currency)>> GetByOwnerAsync(Guid ownerId)
    {
        return await _context.WalletReadModels
            .Where(w => w.OwnerId == ownerId)
            .Select(w => new ValueTuple<Guid, decimal, string>(
                w.WalletId,
                w.CurrentBalance,
                w.Currency
            ))
            .ToListAsync();
    }

    public async Task<(Guid Id, decimal Balance, string Currency)?> GetByIdAndOwnerAsync(Guid walletId, Guid ownerId)
    {
        var wallet = await _context.WalletReadModels
            .Where(w => w.WalletId == walletId && w.OwnerId == ownerId)
            .Select(w => new ValueTuple<Guid, decimal, string>(
                w.WalletId,
                w.CurrentBalance,
                w.Currency
            ))
            .FirstOrDefaultAsync();

        if (wallet == default)
            return null;

        return wallet;
    }

    public async Task<(Guid Id, decimal Balance, string Currency)?> GetByIdAndCurrencyAsync(Guid walletId, string currency)
    {
        var wallet = await _context.WalletReadModels.Where(w => w.WalletId == walletId && w.Currency == currency)
            .Select(w => new ValueTuple<Guid, decimal, string>(
            w.WalletId,
            w.CurrentBalance,
            w.Currency
             ))
    .FirstOrDefaultAsync();

        if (wallet == default)
            return null;

        return wallet;
    }
}
