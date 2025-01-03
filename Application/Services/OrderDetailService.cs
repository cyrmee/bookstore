using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services;

public interface IOrderDetailService
{
    public Task<List<OrderDetailDto>> FindAllAsync();
    public Task<OrderDetailDto> GetOrderDetailAsync(Guid id);
    public Task AddAsync(OrderDetailWriteDto orderDetail);
    public Task AddRangeAsync(List<OrderDetailWriteDto> orderDetails);
    public Task UpdateAsync(OrderDetailDto orderDetail);
    public Task DeleteAsync(Guid id);
    public Task DeleteRangeAsync(List<OrderDetailDto> orderDetails);

    public Task<int> CountAsync();
    public Task<List<OrderDetailDto>> GetPaginatedAsync(int page, int pageSize);
}

public class OrderDetailService(IRepository repository, IMapper mapper) : IOrderDetailService
{
    public async Task<List<OrderDetailDto>> FindAllAsync()
    {
        var orderDetails = await repository.OrderDetail!.FindAllAsync();
        return mapper.Map<List<OrderDetailDto>>(orderDetails);
    }

    public async Task<OrderDetailDto> GetOrderDetailAsync(Guid id)
    {
        var orderDetail = await repository.OrderDetail!.FirstOrDefaultAsync(m => m.Id == id);
        return mapper.Map<OrderDetailDto>(orderDetail);
    }

    public async Task AddAsync(OrderDetailWriteDto orderDetailDto)
    {
        await repository.OrderDetail!.AddAsync(mapper.Map<OrderDetail>(orderDetailDto));
        await repository.SaveAsync();
    }

    public async Task AddRangeAsync(List<OrderDetailWriteDto> orderDetailDtos)
    {
        await repository.OrderDetail!.AddRangeAsync(mapper.Map<List<OrderDetail>>(orderDetailDtos));
        await repository.SaveAsync();
    }

    public async Task UpdateAsync(OrderDetailDto orderDetailDto)
    {
        await repository.OrderDetail!.UpdateAsync(mapper.Map<OrderDetail>(orderDetailDto));
        await repository.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var orderDetailDto = await GetOrderDetailAsync(id);
        await repository.OrderDetail!.DeleteAsync(mapper.Map<OrderDetail>(orderDetailDto));
        await repository.SaveAsync();
    }

    public async Task DeleteRangeAsync(List<OrderDetailDto> orderDetailDtos)
    {
        await repository.OrderDetail!.DeleteRangeAsync(mapper.Map<List<OrderDetail>>(orderDetailDtos));
        await repository.SaveAsync();
    }

    public async Task<int> CountAsync() => await repository.OrderDetail!.CountAsync();

    public async Task<List<OrderDetailDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var orderDetails = await repository.OrderDetail!.GetPaginatedAsync(page, pageSize);
        return mapper.Map<List<OrderDetailDto>>(orderDetails);
    }
}

