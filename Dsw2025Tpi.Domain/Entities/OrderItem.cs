using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        // constructor vacío requerido por Entity Framework para poder instanciar objetos desde la base de datos.
        public OrderItem()
        {
            
        }
        public OrderItem(Guid productId,int quantity, decimal currentUnitPrice)
        {
            ProductId = productId;
           // Product = product;
            UnitPrice = currentUnitPrice;
            Quantity = quantity;
            var subTotal = Subtotal;
        }

        //Forean Key Product
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        //Forean Key Order
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal Subtotal => UnitPrice * Quantity;


    }
}
