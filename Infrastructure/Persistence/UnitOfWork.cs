using MediatR;
using System.Text.Json;
using WalletDemo.Application.Common;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Common;
using WalletDemo.Infrastructure.EventSourcing;
using WalletDemo.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;
    private readonly List<AggregateRoot> _trackedAggregates = new();


    public UnitOfWork(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var efTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return new EfTransaction(efTransaction);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = _trackedAggregates
            .Where(x => x.DomainEvents.Any())
            .ToList();

        Console.WriteLine($"[UOW] Aggregates with events: {domainEntities.Count}");
        var domainEventsWithAggregate = domainEntities
            .SelectMany(a => a.DomainEvents.Select(e => new
            {
                Event = e,
                AggregateType = a.GetType().Name
            }))
            .ToList();


        foreach (var aggregate in domainEntities)
        {
            aggregate.ClearDomainEvents();
        }

        _trackedAggregates.Clear();

        foreach (var item in domainEventsWithAggregate)
        {
            Console.WriteLine($"[UOW] Storing event: {item.GetType().Name}");

            var eventEntity = new EventStoreEntity
            {
                Id = Guid.NewGuid(),
                AggregateId = item.Event.AggregateId,
                AggregateType = item.AggregateType, 
                EventType = item.Event.GetType().Name,
                EventData = JsonSerializer.Serialize(item.Event,item.Event.GetType()),
                OccurredOn = item.Event.OccurredOn
            };

            _context.EventStore.Add(eventEntity);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);

        foreach (var item in domainEventsWithAggregate)
        {
            var domainEvent = item.Event;
            Console.WriteLine($"[UOW] Publishing: {domainEvent.GetType().Name}");

            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(notificationType, domainEvent);

            await _mediator.Publish((INotification)notification!, cancellationToken);
        }

        return result;
    }


    public void Track(AggregateRoot aggregate)
    {
        _trackedAggregates.Add(aggregate);
    }



}
