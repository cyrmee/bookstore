using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;

public class CategoryRepository(BookStoreDbContext context) : GenericRepository<Category>(context), ICategoryRepository
{
    public async Task<IQueryable<Category>> GetAllAsync()
        => await FindAllAsync(false);
}
