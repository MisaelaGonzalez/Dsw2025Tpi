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


namespace Dsw2025Tpi.Application.Services
{
    
    public class OrderManagement : IOrderManagementService
    {
        private readonly IRepository _repository;
        public OrderManagement(IRepository repostory)
        {
            _repository = repostory;
        }

        public async Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request)
        {
          
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
            if (customer == null) 
                throw new EntityNotFoundException($"Cliente no encontrado.");

            // validaciones
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                if (product.IsActive == false)
                    throw new ArgumentException("Producto no disponible, campo IsActive false");
                //if (item.CurrentUnitPrice != product.CurrentUnitPrice)
                  //  throw new ArgumentException("Precio de producto no coincidente");
                if (product == null)
                    throw new EntityNotFoundException($"Producto con ID {item.ProductId} no encontrado.");
                //if (item.Description != product.Description || item.Name != product.Name)
                  //  throw new ArgumentException("Datos de descripcion o nombre no coincidentes");
                if (product.StockQuantity < item.Quantity)
                    throw new ArgumentException($"No hay suficiente stock para el producto {product.Name}.");
                if (item.Quantity <= 0)
                    throw new ArgumentException($"La cantidad del producto debe ser mayor a 0.");
                //if (item.CurrentUnitPrice <= 0)
                  //  throw new ArgumentException($"El precio del producto {item.Name} debe ser mayor a 0.");
                else 
                {
                    product.RestarStock(item.Quantity); 
                    await _repository.Update(product);
                }

            }

                      
            var orderItems = new List<OrderItem>();
            
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                var orderItem = new OrderItem(product.Id, item.Quantity, product.CurrentUnitPrice);
                orderItems.Add(orderItem);
            }
            
            var order = new Order(customer.Id, request.ShippingAddress, request.BillingAddress,
            orderItems, DateTime.UtcNow, OrderStatus.Pending);

            // Guarda la orden en la base de datos.
            var added = await _repository.Add(order);

           
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
