using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;

public class OrderDetailRepository(BookStoreDbContext context) : GenericRepository<OrderDetail>(context), IOrderDetailRepository
{
    public async Task<IQueryable<OrderDetail>> GetAllAsync()
        => await FindAllAsync(false);
}
