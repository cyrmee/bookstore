using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    public Task<IQueryable<Book>> GetAllAsync();
}
