using Proiect_PSSC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PSSC.Models
{
    public record ProductName
    {
        public string Name { get; }
        
        // pentru un nume de produs trebuie sa avem un string mai mare de 1  
        public ProductName(string name)
        {
            if (name.Length > 1)
            {
                Name = name;
            }
            else
            {
                // in caz contrar se arunca exceptia custom definita de noi 
                throw new InvalidProductNameException("Length must be greater than 1");
            }
        }

        // metoda care verifica ca numele produsului este format doar din litere si fara spatii
        public static bool TryParseProductName(string nameString, out ProductName name)
        {
            bool isValid = false;
            name = null;

            if (!string.IsNullOrWhiteSpace(nameString) || nameString.All(char.IsLetter))
            {
                isValid = true;
                name = new ProductName(nameString);
            }

            return isValid;
        }

    }
}