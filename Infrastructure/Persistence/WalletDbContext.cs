using Microsoft.EntityFrameworkCore;
using WalletDemo.Domain.Aggregates;

namespace WalletDemo.Infrastructure.Persistence;

public class WalletDbContext : DbContext
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets => Set<Wallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>(builder =>
        {
            builder.HasKey(w => w.Id);

            builder.OwnsOne(w => w.Balance, money =>
            {
                money.Property(m => m.Amount).HasPrecision(18, 2).HasColumnName("Balance");
                money.Property(m => m.Currency).HasMaxLength(3).HasColumnName("Currency");
            });
        });
    }
}
