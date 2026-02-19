
namespace Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Aggregates.Transfer;
using WalletDemo.Infrastructure.Persistence;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transfer transfer)
    {
        await _context.Transfers.AddAsync(transfer);
    }

    public async Task<Transfer?> GetByIdAsync(Guid id)
    {
        return await _context.Transfers
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

