using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation;

public class Repository(BookStoreDbContext context) : IRepository
{
    private IBookRepository? BookRepository { get; set; }
    private IBookCategoryRepository? BookCategoryRepository { get; set; }
    private ICategoryRepository? CategoryRepository { get; set; }
    private IOrderRepository? OrderRepository { get; set; }
    private IOrderDetailRepository? OrderDetailRepository { get; set; }

    public IBookRepository? Book
    {
        get
        {
            BookRepository ??= new BookRepository(context);
            return BookRepository;
        }
    }

    public IBookCategoryRepository? BookCategory
    {
        get
        {
            BookCategoryRepository ??= new BookCategoryRepository(context);
            return BookCategoryRepository;
        }
    }

    public ICategoryRepository? Category
    {
        get
        {
            CategoryRepository ??= new CategoryRepository(context);
            return CategoryRepository;
        }
    }

    public IOrderRepository? Order
    {
        get
        {
            OrderRepository ??= new OrderRepository(context);
            return OrderRepository;
        }
    }

    public IOrderDetailRepository? OrderDetail
    {
        get
        {
            OrderDetailRepository ??= new OrderDetailRepository(context);
            return OrderDetailRepository;
        }
    }

    public async Task SaveAsync()
        => await context.SaveChangesAsync();
}
