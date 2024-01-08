using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_PSSC.Models;

namespace Proiect_PSSC.Commands
{
    public record ShipOrderCartCommands
    {
        public ShipOrderCartCommands(IReadOnlyCollection<UnvalidatedOrderCartProduct> products)
        {
            InputProducts = products;
        }
        public IReadOnlyCollection<UnvalidatedOrderCartProduct> InputProducts { get; }
    }
}
