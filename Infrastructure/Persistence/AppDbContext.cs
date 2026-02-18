using Microsoft.EntityFrameworkCore;
using WalletDemo.Domain.Aggregates;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<User> Users => Set<User>();

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
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value))
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();


            builder.Property(u => u.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.PasswordHash)
                   .IsRequired();
        });

    }
}
