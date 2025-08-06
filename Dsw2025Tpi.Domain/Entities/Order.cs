using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Order : EntityBase
    {

        public Order()
        {
            
        }
        public Order(Guid customerId, string shippingAddress, string billingAddress, List<OrderItem> orderItems, DateTime createdAt, OrderStatus status)
        {
            CustomerId = customerId;
            ShippingAddress = shippingAddress; //Dirección de envío 
            BillingAddress = billingAddress; //Dirección de facturación
            OrderItems = orderItems;
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            Status = status;
        }
        public Order(Guid customerId, string shippingAddress, string billingAddress,string notes, List<OrderItem> orderItems, DateTime createdAt, OrderStatus status)
        {
            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Notes = notes;
            OrderItems = orderItems;
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            Status = status;
        }

        public OrderStatus Status { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Propiedad calculada: suma de los subtotales de cada ítem.
        // No se guarda en la base, se calcula al momento de leer el objeto.
        public decimal TotalAmount => OrderItems.Sum(item => item.Subtotal);

        //Forean Key Customer
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        //Order Items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        // Relación 1:N → un pedido tiene varios ítems (OrderItem).
        // OrderItems contiene los productos comprados, sus precios y cantidades.

    }
}
