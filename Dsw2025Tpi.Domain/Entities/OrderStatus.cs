using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public enum  OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3, // Enviado
        Delivered = 4, // Entregado
        Cancelled = 5,
        Returned = 6 //Devuelto
    }
}
