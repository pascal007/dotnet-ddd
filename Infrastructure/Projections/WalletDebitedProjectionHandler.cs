using MediatR;
using WalletDemo.Application.Common;
using WalletDemo.Infrastructure.Persistence;


public class WalletDebitedProjectionHandler : INotificationHandler<DomainEventNotification<WalletDebitedEvent>>
{
    private readonly AppDbContext _context;

    public WalletDebitedProjectionHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DomainEventNotification<WalletDebitedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var readModel = await _context.WalletReadModels.FindAsync(domainEvent.WalletId);

        if (readModel != null)
        {
            readModel.CurrentBalance -= domainEvent.Amount;
            readModel.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
