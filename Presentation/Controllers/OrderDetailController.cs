using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrderDetailController(IOrderDetailService orderDetailService) : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService = orderDetailService;

    [HttpGet]
    public async Task<ActionResult<List<OrderDetailDto>>> GetAllOrderDetails()
    {
        var orderDetails = await _orderDetailService.FindAllAsync();
        return Ok(orderDetails);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(Guid id)
    {
        var orderDetail = await _orderDetailService.GetOrderDetailAsync(id);
        return orderDetail == null ? throw new NotFoundException($"OrderDetail with {id} is not found!") : (ActionResult<OrderDetailDto>)Ok(orderDetail);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderDetail(OrderDetailWriteDto orderDetailDto)
    {
        await _orderDetailService.AddAsync(orderDetailDto);
        return CreatedAtAction(nameof(AddOrderDetail), "OrderDetail added successfully!");
    }

    [HttpPost("range")]
    public async Task<IActionResult> AddOrderDetailRange(List<OrderDetailWriteDto> orderDetailDtos)
    {
        await _orderDetailService.AddRangeAsync(orderDetailDtos);
        return CreatedAtAction(nameof(AddOrderDetailRange), "OrderDetails added successfully!");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail(OrderDetailDto orderDetailDto)
    {
        await _orderDetailService.UpdateAsync(orderDetailDto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrderDetail(Guid id)
    {
        _ = await _orderDetailService.GetOrderDetailAsync(id) ?? throw new NotFoundException($"OrderDetail with {id} is not found!");
        await _orderDetailService.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteOrderDetailRange(List<OrderDetailDto> orderDetailDtos)
    {
        foreach (var orderDetailDto in orderDetailDtos)
            _ = await _orderDetailService.GetOrderDetailAsync(orderDetailDto.Id)
                ?? throw new NotFoundException($"OrderDetail with ID {orderDetailDto.Id} is not found!");

        await _orderDetailService.DeleteRangeAsync(orderDetailDtos);
        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetOrderDetailCount()
    {
        var count = await _orderDetailService.CountAsync();
        return Ok(count);
    }

    [HttpGet("paginate")]
    public async Task<ActionResult<List<OrderDetailDto>>> GetPaginatedOrderDetails([FromQuery] int page, [FromQuery] int pageSize)
    {
        var orderDetails = await _orderDetailService.GetPaginatedAsync(page, pageSize);
        return Ok(orderDetails);
    }
}
