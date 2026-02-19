using Microsoft.EntityFrameworkCore.Storage;
using WalletDemo.Application.Interfaces;

namespace WalletDemo.Infrastructure.Persistence;

public class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public EfTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) => _transaction.CommitAsync(cancellationToken);

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
}
