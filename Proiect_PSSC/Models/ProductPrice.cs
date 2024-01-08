using Proiect_PSSC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PSSC.Models
{
    public record ProductPrice
    {
        public decimal Price { get; }

        // pretul produsului trebuie sa fie mai mare decat zero
        public ProductPrice(decimal price)
        {
            if (price > 0)
            {
                Price = price;
            }
            else
            {
                // in caz contrar se arunca exceptia custom definita de noi 
                throw new NegativePriceException("Price must be higher than 0");
            }
        }

        public static bool TryParseProductPrice(string priceString, out ProductPrice price)
        {
            bool isValid = false;
            price = null;

            if (decimal.TryParse(priceString, out decimal numericPrice))
            {
                if (numericPrice > 0)
                {
                    isValid = true;
                    price = new(numericPrice);
                }
            }

            return isValid;
        }
    }
}
