using Proiect_PSSC.Commands;
using Proiect_PSSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proiect_PSSC.Models.ShippedCartEvent;
using static Proiect_PSSC.Models.OrderCartState;
using static Proiect_PSSC.Operations.OrderCartStateOperations;


namespace Proiect_PSSC.Workflow
{
    internal class PayOrderCartWorkflow
    {
        // metoda de Execute apeleaza toate operatiile definite in OrderCartStateOperations
        // dupa fiecare apel de operatie se salveaza starea returnata in orderCartState
        // dupa fiecare apel, orderCartState are o stare noua 
        public IShippedCartEvent Execute(ShipOrderCartCommands command, Func<ProductName, bool> checkProductExists)
        {

            // orderCartState primeste o noua stare cu fiecare apel de metoda 
            EmptyOrderCart emptyOrderCart = new EmptyOrderCart(command.InputProducts);
            IOrderCartState orderCartState = ValidateOrderCart(checkProductExists, emptyOrderCart);
            orderCartState = PayAndOrderProducts(orderCartState);
            orderCartState = BillOrder(orderCartState);
            orderCartState = ShipOrder(orderCartState);

            // Daca a mers totul bine .Match o sa fie in whenShippedOrderCart
            return orderCartState.Match(
                whenEmptyOrderCart: emptyOrderCart => new IShippedCartFailedEvent("Unexpected result") as IShippedCartEvent,
                whenUnvalidatedOrderCart: unvalidatedOrderCart => new IShippedCartFailedEvent(unvalidatedOrderCart.Reason),
                whenValidatedOrderCart: validatedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenPayedOrderCart: payedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenInvoicedOrderCart: invoicedOrderCart => new IShippedCartFailedEvent("Unexpected result"),
                whenShippedOrderCart: shippedOrderCart => new IShippedCartSuccessfulEvent(shippedOrderCart.CheckStatus)
            );
        }
    }
}
