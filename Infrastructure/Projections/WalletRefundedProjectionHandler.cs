
using MediatR;
using Microsoft.EntityFrameworkCore;
using WalletDemo.Application.Common;
using WalletDemo.Domain.Events;
using WalletDemo.Infrastructure.Persistence;

namespace WalletDemo.Infrastructure.Projections;

public class WalletRefundedProjectionHandler : INotificationHandler<DomainEventNotification<WalletRefundedEvent>>
{
    private readonly AppDbContext _context;

    public WalletRefundedProjectionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DomainEventNotification<WalletRefundedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var wallet = await _context.WalletReadModels
            .FirstOrDefaultAsync(w => w.WalletId == domainEvent.WalletId);

        if (wallet == null)
            return;

        wallet.CurrentBalance += domainEvent.Amount;
        wallet.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}