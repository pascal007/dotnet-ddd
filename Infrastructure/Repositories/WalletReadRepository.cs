using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Interfaces;
using WalletDemo.Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class WalletReadRepository : IWalletReadRepository
{
    private readonly AppDbContext _context;

    public WalletReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> GetBalanceAsync(Guid walletId, Guid UserId)
    {
        var readModel = await _context.WalletReadModels.FirstOrDefaultAsync(x => x.WalletId == walletId && x.OwnerId == UserId);

        return readModel?.CurrentBalance;
    }
}
