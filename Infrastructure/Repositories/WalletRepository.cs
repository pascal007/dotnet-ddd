using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates;
using WalletDemo.Infrastructure.Persistence;

namespace WalletDemo.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _context;

    public WalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
    }

    public async Task<Wallet?> GetByCurrencyAndOwner(string currency, string owner)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.Balance.Currency == currency && w.Owner == owner);
    }

    public async Task<Wallet?> GetByIdAndOwnerAsync(Guid id, string owner)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.Owner == owner);
    }

    public async Task<Wallet?> GetByIdAsync(Guid id)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Wallet>?> GetByOwnerAsync(string owner)
    {
        return await _context.Wallets.Where(w => w.Owner == owner).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
