using Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Wallets.Projections;
using WalletDemo.Domain.Aggregates.Transfer;
using WalletDemo.Domain.ValueObjects;
using WalletDemo.Infrastructure.EventSourcing;

namespace WalletDemo.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<WalletReadModel> WalletReadModels { get; set; }
    public DbSet<Transfer> Transfers => Set<Transfer>();
    public DbSet<EventStoreEntity> EventStore => Set<EventStoreEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

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



        modelBuilder.Entity<Transfer>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.FromWalletId)
                   .IsRequired();

            builder.Property(t => t.ToWalletId)
                   .IsRequired();

            builder.Property(t => t.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(t => t.Status)
                   .HasConversion<string>() 
                   .IsRequired();
        });

        modelBuilder.Entity<EventStoreEntity>(builder =>
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AggregateType).IsRequired();
            builder.Property(e => e.EventType).IsRequired();
            builder.Property(e => e.EventData).IsRequired();
        });

        modelBuilder.Entity<WalletReadModel>(builder =>
        {
            builder.HasKey(w => w.WalletId);

            builder.Property(w => w.Currency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(w => w.CurrentBalance)
                .HasPrecision(18, 2);

            builder.Property(w => w.OwnerId)
                .IsRequired();
        });



    }
}
