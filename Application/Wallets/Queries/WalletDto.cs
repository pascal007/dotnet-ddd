namespace WalletDemo.Application.Wallets.Queries;

public class WalletDto
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = default!;
}
