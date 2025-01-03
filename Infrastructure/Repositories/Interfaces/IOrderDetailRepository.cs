using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
{
    public Task<IQueryable<OrderDetail>> GetAllAsync();
}
