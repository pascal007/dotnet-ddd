namespace WalletDemo.Application.Wallets.Projections;

public class WalletReadModel
{
    public Guid WalletId { get; set; }
    public decimal CurrentBalance { get; set; }
    public string Currency { get; set; } = default!;
    public Guid OwnerId { get; set; } = default!;
    public DateTime LastUpdated { get; set; }
}
