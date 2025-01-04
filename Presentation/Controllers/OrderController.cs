using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.FindAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrderAsync(id);
        return order == null ? throw new NotFoundException($"Order with {id} is not found!") : (ActionResult<OrderDto>)Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(OrderWriteDto orderDto)
    {
        await _orderService.AddAsync(orderDto);
        return CreatedAtAction(nameof(AddOrder), "Order added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddOrderRange(List<OrderWriteDto> orderDtos)
    {
        await _orderService.AddRangeAsync(orderDtos);
        return CreatedAtAction(nameof(AddOrderRange), "Orders added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder(OrderDto orderDto)
    {
        await _orderService.UpdateAsync(orderDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        _ = await _orderService.GetOrderAsync(id) ?? throw new NotFoundException($"Order with {id} is not found!");
        await _orderService.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteOrderRange(List<OrderDto> orderDtos)
    {
        foreach (var orderDto in orderDtos)
            _ = await _orderService.GetOrderAsync(orderDto.Id)
                ?? throw new NotFoundException($"Order with ID {orderDto.Id} is not found!");

        await _orderService.DeleteRangeAsync(orderDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetOrderCount()
    {
        var count = await _orderService.CountAsync();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<OrderDto>>> GetPaginatedOrders([FromQuery] int page, [FromQuery] int pageSize)
    {
        var orders = await _orderService.GetPaginatedAsync(page, pageSize);
        return Ok(orders);
    }
}
