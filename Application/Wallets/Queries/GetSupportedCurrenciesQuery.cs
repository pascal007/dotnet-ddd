using MediatR;

namespace WalletDemo.Application.Wallets.Queries;

public record GetSupportedCurrenciesQuery() : IRequest<List<string>>;
