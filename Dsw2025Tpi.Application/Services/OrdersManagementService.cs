using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static Dsw2025Tpi.Application.Dtos.OrderModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

// Contiene la lógica de negocio. Es la capa intermedia entre los controladores
// (si usás Web API) y los datos (repositorio/basededatos).
namespace Dsw2025Tpi.Application.Services
{
    // Este código implementa la lógica de negocio para agregar una nueva orden de compra (pedido) en una aplicación
    public class OrderManagement : IOrderManagementService
    {
        private readonly IRepository _repository;
        public OrderManagement(IRepository repostory)
        {
            _repository = repostory;
        }

        public async Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request)
        {
            // Verifica que: Se haya proporcionado un ID de cliente válido.
            // Las direcciones de envío y facturación no estén vacías.
          
            if (request.CustomerId == Guid.Empty ||
                string.IsNullOrWhiteSpace(request.ShippingAddress) ||
                string.IsNullOrWhiteSpace(request.BillingAddress))

            {
                throw new ArgumentException("Ingrese dirección de envio y/o facturación");
            }

            // Validar que la lista de OrderItems no esté vacía
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                throw new ArgumentException("La orden debe tener al menos un producto.");

            // Validar existencia del cliente
            var customer = await _repository.GetById<Customer>(request.CustomerId);
            if (customer == null) // si el cliente no existe lanza excepcion
                throw new EntityNotFoundException($"Cliente no encontrado.");

            // validaciones
            foreach (var item in request.OrderItems)
            {
                /* Por cada producto en la orden, valida:
                    Que el producto exista.
                    Que esté activo (IsActive == true).
                    Que el precio enviado coincida con el actual.
                    Que nombre y descripción coincidan.
                    Que haya suficiente stock.
                   Que cantidad y precio sean mayores a cero.*/
                var product = await _repository.GetById<Product>(item.ProductId);
                if (product.IsActive == false)
                    throw new ArgumentException("Producto no disponible, campo IsActive false");
                //if (item.CurrentUnitPrice != product.CurrentUnitPrice)
                  //  throw new ArgumentException("Precio de producto no coincidente");
                if (product == null)
                    throw new EntityNotFoundException($"Producto con ID {item.ProductId} no encontrado.");
                //if (item.Description != product.Description || item.Name != product.Name)
                  //  throw new ArgumentException("Datos de descripcion o nombre no coincidentes");
                if (product.StockCuantity < item.Quantity)
                    throw new ArgumentException($"No hay suficiente stock para el producto {product.Name}.");
                if (item.Quantity <= 0)
                    throw new ArgumentException($"La cantidad del producto debe ser mayor a 0.");
                //if (item.CurrentUnitPrice <= 0)
                  //  throw new ArgumentException($"El precio del producto {item.Name} debe ser mayor a 0.");
                else // Si pasa todas las validaciones:
                   // Se resta el stock del producto y se actualiza en la base de datos.
                {
                    product.RestarStock(item.Quantity); // Restar la cantidad del producto del stock
                    await _repository.Update(product);
                }

            }

            // crea objetos de tipo OrderItem con los datos del producto y los agrega a la lista.            
            var orderItems = new List<OrderItem>();
            //Crear los items de la orden
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                var orderItem = new OrderItem(product.Id, item.Quantity, product.CurrentUnitPrice);
                orderItems.Add(orderItem);
            }
            // Usa el cliente, las direcciones y los OrderItems para crear un nuevo objeto Order.
            //Estado inicial: Pending.
            // Crear la orden
            var order = new Order(customer.Id, request.ShippingAddress, request.BillingAddress,
            orderItems, DateTime.UtcNow, OrderStatus.Pending);

            // Guarda la orden en la base de datos.
            var added = await _repository.Add(order);

            // Devuelve un OrderModel.OrderResponse con:
            // IDs, direcciones, fecha, estado.
            // El total.
            // La lista de ítems de la orden convertidos a OrderItemResponse.
            return new OrderModel.OrderResponse(
                added.Id,
                added.CustomerId,
                added.ShippingAddress,
                added.BillingAddress,
                added.CreatedAt,
                added.TotalAmount,
                added.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                    oi.ProductId,
                    oi.Product?.Name ?? "",
                    oi.Product?.Description ?? "",
                    oi.UnitPrice,
                    oi.Quantity,
                    oi.Subtotal)).ToList(),
                added.Status.ToString());
            
        }

        public async Task<IEnumerable<OrderModel.OrderResponse>> GetOrdersAsync(string? status, Guid? customerId, int pageNumber, int pageSize)
        {

            var allOrders = await _repository.GetAll<Order>(includeProperties: "OrderItems.Product");

            // Filtros
            if (!string.IsNullOrEmpty(status))
                allOrders = allOrders.Where(o => o.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));

            if (customerId.HasValue)
                allOrders = allOrders.Where(o => o.CustomerId == customerId.Value);

            // Paginación
            var pagedOrders = allOrders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map a OrderResponse
            return pagedOrders.Select(order => new OrderResponse(
                order.Id,
                order.CustomerId,
                order.ShippingAddress,
                order.BillingAddress,
                order.CreatedAt,
                order.TotalAmount,
                order.OrderItems.Select(oi => new OrderItemResponse(
                    oi.ProductId,
                    oi.Product?.Name ?? "",
                    oi.Product?.Description ?? "",
                    oi.UnitPrice,
                    oi.Quantity,
                    oi.Subtotal
                )).ToList(),
                order.Status.ToString()
            ));
        }

        public async Task<OrderModel.OrderResponse> GetOrderById(Guid id)
        {
            var order = await _repository.GetById<Order>(id, "OrderItems.Product");
            
            if (order == null) throw new EntityNotFoundException("Order no encontrada");

            return new OrderModel.OrderResponse(
                order.Id,
                order.CustomerId,
                order.ShippingAddress,
                order.BillingAddress,
                order.CreatedAt,
                order.TotalAmount,
                order.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                    oi.ProductId,
                    oi.Product?.Name ?? "",
                    oi.Product?.Description ?? "",
                    oi.UnitPrice,
                    oi.Quantity,
                    oi.Subtotal)).ToList(),
                order.Status.ToString()
             );
        }

        public async Task<OrderModel.OrderResponse> UpdateOrderStatusAsync(Guid id, string NewStatus)
        {
            var order = await _repository.GetById<Order>(id, "OrderItems.Product");
            if (order == null) throw new EntityNotFoundException("Order no encontrada");

            if (!Enum.TryParse<OrderStatus>(NewStatus, true, out var newParsedStatus))
                throw new ArgumentException("Estado inválido.");

            // Validar transición válida 
            var estadosValidos = new[]
            {
               OrderStatus.Pending,
               OrderStatus.Processing,
               OrderStatus.Shipped,
               OrderStatus.Delivered,
               OrderStatus.Cancelled

            };

            if (!estadosValidos.Contains(newParsedStatus))
                throw new ArgumentException("Transición de estado no permitida.");
            
            // Actualizar solo si es distinto
            if (order.Status != newParsedStatus)
            {
                order.Status = newParsedStatus;
                await _repository.Update(order);
            }

            return new OrderModel.OrderResponse(
                order.Id,
                order.CustomerId,
                order.ShippingAddress,
                order.BillingAddress,
                order.CreatedAt,
                order.TotalAmount,
                order.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                    oi.ProductId,
                    oi.Product?.Name ?? "",
                    oi.Product?.Description ?? "",
                    oi.UnitPrice,
                    oi.Quantity,
                    oi.Subtotal)).ToList(),
                order.Status.ToString()
             );

        }
    }
}
