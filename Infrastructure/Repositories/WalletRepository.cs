using Domain.Aggregates.Wallet;
using Infrastructure.EventSourcing;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Infrastructure.EventSourcing;
using WalletDemo.Infrastructure.Persistence;

namespace WalletDemo.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _appDbContext;

    public WalletRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

    }

    public async Task<bool> ExistsAsync(string currency, Guid owner)
    {
        return await _appDbContext.WalletReadModels
            .AnyAsync(w => w.Currency == currency && w.OwnerId == owner);
    }

    public async Task<Wallet?> GetByIdAsync(Guid id)
    {
        var events = await _appDbContext.EventStore.Where(e => e.AggregateId == id &&
                        e.AggregateType == "Wallet")
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();

        if (!events.Any())
            return null;

        var domainEvents = events
            .Select(e => Deserialize(e))
            .ToList();


        return Wallet.Rehydrate(domainEvents);
    }

    private IDomainEvent Deserialize(EventStoreEntity entity)
    {
        return entity.EventType switch
        {
            nameof(WalletCreatedEvent) =>
                JsonSerializer.Deserialize<WalletCreatedEvent>(entity.EventData)!,

            nameof(WalletCreditedEvent) =>
                JsonSerializer.Deserialize<WalletCreditedEvent>(entity.EventData)!,

            nameof(WalletDebitedEvent) =>
                JsonSerializer.Deserialize<WalletDebitedEvent>(entity.EventData)!,

            nameof(WalletRefundedEvent) =>
            JsonSerializer.Deserialize<WalletRefundedEvent>(entity.EventData)!,

            _ => throw new InvalidOperationException(
                $"Unknown event type: {entity.EventType}")
        };
    }


}
