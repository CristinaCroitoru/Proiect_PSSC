using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace Proiect_PSSC.Models
{
    [AsChoice]
    public static partial class ShippedCartEvent
    {
      /* se defineste o interfata IShippedCartEvent care urmeaza sa fie implementata de starile: 
      IShippedCartSuccessfulEvent sau IShippedCartFailedEvent care vor fi ulterior utilizate in clasa PayOrderCartWorkflow /worflow ul nostru
      */

        public interface IShippedCartEvent { }

        public record IShippedCartSuccessfulEvent : IShippedCartEvent
        {
            public string Reason { get; }

            public IShippedCartSuccessfulEvent(string reason)
            {
                Reason = reason;
            }
        }

        public record IShippedCartFailedEvent : IShippedCartEvent
        {
            public string Reason { get; }

            public IShippedCartFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
