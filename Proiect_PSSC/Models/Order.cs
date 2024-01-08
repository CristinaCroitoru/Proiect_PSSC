using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_PSSC.Models
{
    // fiecare order se presupune ca are un ContactPerson, PhoneNumber si un DeliveryAddress
    // de asemenea fiecare comanda ar trebui sa aiba asociata o lista de <ProductName, ProductPrice> 
    public record Order
    {
        public string ProductName { get; init; } 
        public string ProductPrice { get; init; }

        public string ContactPerson { get; init; }
        public string PhoneNumber { get; init; }
        public string DeliveryAddress { get; init; }
        
    }

}
