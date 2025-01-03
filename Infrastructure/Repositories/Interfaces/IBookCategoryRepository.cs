using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IBookCategoryRepository : IGenericRepository<BookCategory>
{
    public Task<IQueryable<BookCategory>> GetAllAsync();
}
