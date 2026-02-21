
namespace WalletDemo.Infrastructure.Projections;

using MediatR;
using System;
using WalletDemo.Application.Common;
using WalletDemo.Application.Wallets.Projections;
using WalletDemo.Domain.Events;
using WalletDemo.Infrastructure.Persistence;


public class WalletCreatedProjectionHandler : INotificationHandler<DomainEventNotification<WalletCreatedEvent>>
{
    private readonly AppDbContext _context;

    public WalletCreatedProjectionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DomainEventNotification<WalletCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var readModel = new WalletReadModel
        {
            WalletId = domainEvent.WalletId,
            OwnerId = domainEvent.Owner,   
            CurrentBalance = 0,
            Currency = domainEvent.Currency,
            LastUpdated = DateTime.UtcNow
        };

        _context.WalletReadModels.Add(readModel);
        await _context.SaveChangesAsync(cancellationToken);

    }
}

