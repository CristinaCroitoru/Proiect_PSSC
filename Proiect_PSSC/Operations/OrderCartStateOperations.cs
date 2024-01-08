using Proiect_PSSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proiect_PSSC.Models.OrderCartState;

namespace Proiect_PSSC.Operations
{

    // operatiile asociate starilor definite in fisierul OrderCartState
    // ele sunt apelate in workflow
    public static class OrderCartStateOperations
    {
        // validarea efectiva a produselor - aceasta se face utilizand metodele TryParse de la fiecare clasa  
        public static IOrderCartState ValidateOrderCart(Func<ProductName, bool> checkProductExists, EmptyOrderCart orderCart)
        {
            // lista de produse care urmeaza sa fie validata 
            List<ValidatedOrderCartProduct> validatedProducts = new List<ValidatedOrderCartProduct>();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var product in orderCart.Products)
            {
                if (!ProductName.TryParseProductName(product.productName, out ProductName productName))
                {
                    invalidReson = $"Invalid product name: {product.productName}";
                    isValidList = false;
                    break;
                }
                if (!ProductPrice.TryParseProductPrice(product.price, out ProductPrice productPrice))
                {
                    invalidReson = $"Invalid product price: {product.price}";
                    isValidList = false;
                    break;
                }
                ValidatedOrderCartProduct validatedOrderCartProduct = new(productName, productPrice);
                validatedProducts.Add(validatedOrderCartProduct);
            }

            if (isValidList)
            {
                Console.WriteLine($"Products successfully validated - ValidateOrderCart");
                return new ValidatedOrderCart(validatedProducts);
            }
            else
            {
                return new UnvalidatedOrderCart(orderCart.Products, invalidReson);
            }
        }

        /*
         Pentru operatiile PayAndOrderProducts, BillOrder si ShipOrder :
     vom defini atatea stari in Match-ul de la orderCart cate avem in clasa OrderCartState
        
         whenValidatedOrderCart -> whenPayedOrderCart - ordinea din Match 
         when-urile din Match au corepondenta o operatie 
         de exemplu whenShippedOrderCart <=> ShipOrder

         implemenarea noii stari are loc in handlerul aferent vechii stari,adica trecerea in noua stare este scrisa in evenimentul anterior 
        */


        // preluare comanda 
        public static IOrderCartState PayAndOrderProducts(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => 
                {
                 // calcularea pretului total al comenzii se face in whenValidatedOrderCart care este anterior starii whenPayedOrderCart
                    decimal total = 0;
                    foreach (var product in validatedOrderCart.OrderCartProducts)
                    {
                        total += product.price.Price;
                    }

                    Console.WriteLine($"Order Price: {total} - PayAndOrderProducts");
                    PayedOrderCart payedShoppingCart = new(validatedOrderCart.OrderCartProducts, total);
                    return payedShoppingCart;
                },

                whenPayedOrderCart: payedOrderCart => payedOrderCart,
                whenInvoicedOrderCart: invoicedOrderCart => invoicedOrderCart,
                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );

    
        // facturare comanda 
        public static IOrderCartState BillOrder(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => validatedOrderCart,

                whenPayedOrderCart: payedOrderCart =>
                {
                    // definim daca putem sau nu confirma comanda pentru facturare
                    var total = payedOrderCart.Total;
                    var confirmation = false;
                    if (total > 0)
                    {
                        confirmation = true;
                    }

                    Console.WriteLine($"Order was successfully billed: {confirmation} - BillOrder");
                    InvoicedOrderCart invoicedOrder = new(confirmation);
                    return invoicedOrder;
                },

                whenInvoicedOrderCart: invoicedOrderCart => invoicedOrderCart,
                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );


        // expediere comanda 
        public static IOrderCartState ShipOrder(IOrderCartState orderCart) =>
            orderCart.Match(
                whenEmptyOrderCart: emptyOrderCart => emptyOrderCart,
                whenUnvalidatedOrderCart: unvalidatedCardCart => unvalidatedCardCart,
                whenValidatedOrderCart: validatedOrderCart => validatedOrderCart,
                whenPayedOrderCart: payedOrderCart => payedOrderCart,

                whenInvoicedOrderCart: invoicedOrderCart => {

                    // definim daca comanda poate fi expediata sau nu 
                    var isStatusReady = "The Order has failed to be shipped - ShipOrder";
                    var confirmation = invoicedOrderCart.ConfirmBilling;
                    if (confirmation == true)
                    {
                        isStatusReady = "Order successfully shipped - ShipOrder";
                    }

                    ShippedOrderCart shippedOrder = new(isStatusReady);
                    return shippedOrder;
                },

                whenShippedOrderCart: shippedOrderCart => shippedOrderCart
                );

    }

}
