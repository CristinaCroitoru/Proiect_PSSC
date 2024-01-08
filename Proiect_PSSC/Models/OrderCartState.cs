using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PSSC.Models
{
    [AsChoice]
    public static partial class OrderCartState
    {
        /* se defineste o interfata IOrderCartState care urmeaza sa fie implementata de starile in care poate fi o comanda
        in cadrul aplicatiei noastre o comanda/order poate sa fie in starile urmatoare: 
           - EmptyOrderCart - comanda goala, adica cosul e gol
           - UnvalidatedOrderCart - comanda nevalidata ,produsele din cos nu sunt validate 
           - ValidatedOrderCart - comanda validata, produsele din cos au fost validate 
           - PayedOrderCart - comanda plasata/preluata si platita (cand comanda e platita devine plasata)
           - InvoicedOrderCart - comanda facturata (cand comanda e plasata urmeaza sa fie si facturata)
           - ShippedOrderCart - comanda expediata (ultimul state/stare a comenzii este expediata dupa ce a fost facturata)

        Pentru fiecare stare a unei comenzi exista o operatie in OrderCartStateOperations prin care se poate ajunge 
    in starea respectiva dintr o stare anterioara
        De exemplu: pentru comanda expediata (ShippedOrderCart) -> exista operatia ShipOrder, care ne aduce din faza de 
        billed in starea de shipped
       */

        public interface IOrderCartState { }
       
        public record EmptyOrderCart : IOrderCartState
        {
            public EmptyOrderCart(IReadOnlyCollection<UnvalidatedOrderCartProduct> products)
            {
                Products = products;
            }

            public IReadOnlyCollection<UnvalidatedOrderCartProduct> Products { get; }
        }

        public record UnvalidatedOrderCart : IOrderCartState
        {
            public UnvalidatedOrderCart(IReadOnlyCollection<UnvalidatedOrderCartProduct> orderCartProducts, string reason)
            {
                OrderCartProducts = orderCartProducts;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedOrderCartProduct> OrderCartProducts { get; }
            // reason = motivul pentru care comanda este invalida 
            public string Reason { get; }
        }

        public record ValidatedOrderCart : IOrderCartState
        {

            public ValidatedOrderCart(IReadOnlyCollection<ValidatedOrderCartProduct> orderCartProducts)
            {
                OrderCartProducts = orderCartProducts;
            }
            // este nevoie de o lista de produse care sa fie validate 
            public IReadOnlyCollection<ValidatedOrderCartProduct> OrderCartProducts { get; }
        }

        // payed = comandat / plasat
        public record PayedOrderCart : IOrderCartState
        {
            public PayedOrderCart(IReadOnlyCollection<ValidatedOrderCartProduct> orderCartProducts, decimal total)
            {
                OrderCartProducts = orderCartProducts;
                Total = total;
                // totalul care se calculeaza pe baza produselor care au fost validate 
                // acest lucru se face in operatia asociata starii 
            }

            public IReadOnlyCollection<ValidatedOrderCartProduct> OrderCartProducts { get; }
            public decimal Total { get; }
        }

        
        public record InvoicedOrderCart : IOrderCartState
        {
            public InvoicedOrderCart(bool confirmation)
            {
                ConfirmBilling = confirmation;
            }
            public bool ConfirmBilling { get; }
            // o variabila cu scopul de tine minte daca comanda a fost facturata sau nu, daca a fost facturata cu succes =>  ConfirmBilling = true
        }


        public record ShippedOrderCart : IOrderCartState
        {

            public ShippedOrderCart(string statusLivrare)
            {
                CheckStatus = statusLivrare;
            }

            public string CheckStatus { get; }
            // statusul comenzii - daca a fost sau nu livrata
        }
       
    }
}