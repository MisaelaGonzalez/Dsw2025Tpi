using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private IOrderManagementService _orderManagmentService;
        public OrdersController(IOrderManagementService orderManagement)
        {
            _orderManagmentService = orderManagement;
        }

        //Agregar una orden
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderModel.OrderRequest request)
        {
            try
            {
                var order = await _orderManagmentService.AddOrder(request);
                return Created("api/orders", order);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound(enfe.Message);
            }
            catch (DuplicatedEntityException de)
            {
                return Conflict(de.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            } 
        }

        // Obtener órdenes 
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? status,
            [FromQuery] Guid? customerId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderManagmentService.GetOrdersAsync(status, customerId, pageNumber, pageSize);

                if (orders == null || !orders.Any())
                    return NoContent();

                return Ok(orders);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        //Obtener orden por id
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var order = await _orderManagmentService.GetOrderById(id);
                return Ok(order);
            }
            catch(EntityNotFoundException enft)
            {
                return NotFound(enft.Message); // 404
            }
            catch (Exception e)
            {
                return Problem("Se produjo un error al obtener el producto, {0}", e.Message);
            }
        }

        //Modificar estado de una orden
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderModel.StatusUpdateRequest request)
        {
            try
            {
                var order = await _orderManagmentService.UpdateOrderStatusAsync(id, request.NewStatus);
                return Ok(order);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

    }
}
