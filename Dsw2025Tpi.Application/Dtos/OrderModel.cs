using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Contiene modelos simplificados que se usan para enviar y recibir datos entre capas (por ejemplo, entre la capa de aplicación y la API o el cliente).
// Este código define un conjunto de tipos de datos inmutables (record) agrupados en una clase contenedora llamada OrderModel. Está organizado en
// solicitudes (Requests) y respuestas (Responses) para trabajar con órdenes de compra o pedidos dentro de una aplicación,

namespace Dsw2025Tpi.Application.Dtos
{
    // Aunque los record suelen usarse para definir tipos inmutables (una vez creados no se pueden cambiar),
    // aquí se usa como un contenedor de otros records internos.
    public record OrderModel
    {
        //Requests
        //Se usa para crear o enviar una orden nueva
        public record OrderRequest(Guid CustomerId, string ShippingAddress, string BillingAddress, List<OrderItemModel> OrderItems);
        //Representa un ítem individual dentro del pedido.
        public record OrderItemModel(Guid ProductId, int Quantity);
        // string Name, string Description, decimal CurrentUnitPrice
        public record StatusUpdateRequest(string NewStatus);

        //Responses
        // Se usa para devolver información de un pedido existente.
        public record OrderResponse(Guid Id,
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            DateTime Date,
            decimal TotalAmount,
            List<OrderItemResponse> OrderItems,
            string Status);
        //Representa la respuesta de un ítem del pedido
        public record OrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPriceint, int Quantity, decimal Subtotal);


    }
}
