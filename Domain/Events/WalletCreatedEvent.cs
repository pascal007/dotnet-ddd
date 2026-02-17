using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletDemo.Domain.Events;

public record WalletCreatedEvent(Guid WalletId, string Owner, string Currency);
