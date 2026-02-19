using MediatR;
using System;
using WalletDemo.Application.Common;
using WalletDemo.Infrastructure.Persistence;


public class WalletCreditedProjectionHandler : INotificationHandler<DomainEventNotification<WalletCreditedEvent>>
{
    private readonly AppDbContext _context;

    public WalletCreditedProjectionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DomainEventNotification<WalletCreditedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var readModel = await _context.WalletReadModels.FindAsync(domainEvent.WalletId);

        if (readModel != null)
        {
            readModel.CurrentBalance += domainEvent.Amount;
            readModel.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
