using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IQueryable<Order>> GetAllAsync();
}
