using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Services
{
    public interface IOrderManagementService
    {
        Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request);
        Task<IEnumerable<OrderModel.OrderResponse>> GetOrdersAsync(string? status, Guid? customerId, int pageNumber, int pageSize);

        Task<OrderModel.OrderResponse> GetOrderById(Guid id);

        Task<OrderModel.OrderResponse> UpdateOrderStatusAsync(Guid id, string NewStatus);
    }
}