
using System.Text.Json;
using WalletDemo.Domain.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Infrastructure.EventSourcing;
using WalletDemo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EventSourcing;


public class EventStoreRepository
{
    private readonly AppDbContext _context;

    public EventStoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<IDomainEvent>> GetEventsAsync(Guid aggregateId)
    {
        var storedEvents = await _context.EventStore
            .Where(e => e.AggregateId == aggregateId && e.AggregateType == "Wallet")
            .OrderBy(e => e.OccurredOn)
            .ToListAsync();

        return storedEvents
            .Select(DeserializeEvent)
            .ToList();
    }

    private IDomainEvent DeserializeEvent(EventStoreEntity entity)
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

            _ => throw new InvalidOperationException("Unknown event type")
        };
    }
}
