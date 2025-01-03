using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;
public class OrderRepository(BookStoreDbContext context) : GenericRepository<Order>(context), IOrderRepository
{
    public async Task<IQueryable<Order>> GetAllAsync()
        => await FindAllAsync(false);
}
