
using MediatR;
using WalletDemo.Domain.Common;

namespace WalletDemo.Application.Wallets.Queries;

public class GetSupportedCurrenciesHandler : IRequestHandler<GetSupportedCurrenciesQuery, List<string>>{


    public Task<List<string>> Handle(GetSupportedCurrenciesQuery query, CancellationToken cancellationToken)
    {
        var currencies = new List<string>() { Currencies.GBP, Currencies.USD};

        return Task.FromResult(currencies);
    }


}

