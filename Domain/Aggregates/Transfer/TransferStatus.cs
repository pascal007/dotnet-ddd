
namespace WalletDemo.Domain.Aggregates.Transfer;

public enum TransferStatus
{
    Initiated,
    Debited,
    Credited,
    Completed,
    Failed,
    Compensated
}
