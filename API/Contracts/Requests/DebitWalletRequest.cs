namespace WalletDemo.API.Contracts.Requests;

public record DebitWalletRequest (string Currency, decimal Amount);