using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PSSC.Exceptions
{
    internal class InvalidProductNameException : Exception
    {
        public InvalidProductNameException(string message) : base(message) { }
    }
}
