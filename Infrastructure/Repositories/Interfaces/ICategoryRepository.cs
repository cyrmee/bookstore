using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    public Task<IQueryable<Category>> GetAllAsync();
}
