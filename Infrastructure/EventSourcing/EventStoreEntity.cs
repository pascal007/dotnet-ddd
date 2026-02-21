
namespace WalletDemo.Infrastructure.EventSourcing;

public class EventStoreEntity
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public string EventData { get; set; } = default!;
    public DateTime OccurredOn { get; set; }
}
