using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services;

public interface IOrderService
{
    public Task<List<OrderDto>> FindAllAsync();
    public Task<OrderDto> GetOrderAsync(Guid id);
    public Task AddAsync(OrderWriteDto order);
    public Task AddRangeAsync(List<OrderWriteDto> orders);
    public Task UpdateAsync(OrderDto order);
    public Task DeleteAsync(Guid id);
    public Task DeleteRangeAsync(List<OrderDto> orders);

    public Task<int> CountAsync();
    public Task<List<OrderDto>> GetPaginatedAsync(int page, int pageSize);
}

public class OrderService(IRepository repository, IMapper mapper) : IOrderService
{
    public async Task<List<OrderDto>> FindAllAsync()
    {
        var orders = await repository.Order!.FindAllAsync();
        return mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        var order = await repository.Order!.FirstOrDefaultAsync(m => m.Id == id);
        return mapper.Map<OrderDto>(order);
    }

    public async Task AddAsync(OrderWriteDto orderDto)
    {
        await repository.Order!.AddAsync(mapper.Map<Order>(orderDto));
        await repository.SaveAsync();
    }

    public async Task AddRangeAsync(List<OrderWriteDto> orderDtos)
    {
        await repository.Order!.AddRangeAsync(mapper.Map<List<Order>>(orderDtos));
        await repository.SaveAsync();
    }

    public async Task UpdateAsync(OrderDto orderDto)
    {
        await repository.Order!.UpdateAsync(mapper.Map<Order>(orderDto));
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var orderDto = await GetOrderAsync(id);
        await repository.Order!.DeleteAsync(mapper.Map<Order>(orderDto));
        await repository.SaveAsync();
    }

    public async Task DeleteRangeAsync(List<OrderDto> orderDtos)
    {
        await repository.Order!.DeleteRangeAsync(mapper.Map<List<Order>>(orderDtos));
        await repository.SaveAsync();
    }

    public async Task<int> CountAsync() => await repository.Order!.CountAsync();

    public async Task<List<OrderDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var orders = await repository.Order!.GetPaginatedAsync(page, pageSize);
        return mapper.Map<List<OrderDto>>(orders);
    }
}

