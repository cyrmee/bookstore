using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;

public class BookRepository(BookStoreDbContext context) : GenericRepository<Book>(context), IBookRepository
{
    public async Task<IQueryable<Book>> GetAllAsync()
        => await FindAllAsync(false);
}
