namespace WalletDemo.API.Contracts.Requests;



public record TransferFundRequest(Guid sourceWallet, Guid DestinationWallet, decimal Amount);
