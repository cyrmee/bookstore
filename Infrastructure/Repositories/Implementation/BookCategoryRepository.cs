using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;

public class BookCategoryRepository(BookStoreDbContext context) : GenericRepository<BookCategory>(context), IBookCategoryRepository
{
    public async Task<IQueryable<BookCategory>> GetAllAsync()
        => await FindAllAsync(false);
}
