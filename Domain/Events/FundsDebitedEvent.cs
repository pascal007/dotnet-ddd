using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Domain.Events;

public record FundsDebitedEvent(Guid WalletId, Money Amount);
