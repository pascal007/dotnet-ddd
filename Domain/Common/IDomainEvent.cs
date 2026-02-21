
namespace WalletDemo.Domain.Common;

public interface IDomainEvent
{
    Guid AggregateId { get; }

    DateTime OccurredOn { get; }

}
