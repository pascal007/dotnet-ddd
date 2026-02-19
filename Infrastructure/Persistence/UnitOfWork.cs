using MediatR;
using WalletDemo.Application.Common;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Common;
using WalletDemo.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

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
        var domainEntities = _context.ChangeTracker
            .Entries()
            .Where(x => x.Entity is AggregateRoot ar && ar.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => ((AggregateRoot)x.Entity).DomainEvents)
            .ToList();

        Console.WriteLine($"[UOW] Domain events found: {domainEvents.Count}");


        var result = await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            Console.WriteLine($"[UOW] Publishing: {domainEvent.GetType().Name}");

            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(
                notificationType,
                domainEvent);

            await _mediator.Publish((INotification)notification!, cancellationToken);
        }

        foreach (var entityEntry in domainEntities)
        {
            var aggregate = (AggregateRoot)entityEntry.Entity;
            aggregate.ClearDomainEvents();
        }


        return result;
    }

}
