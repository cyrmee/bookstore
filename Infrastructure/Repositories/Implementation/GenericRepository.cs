using Domain.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Implementation;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
{
    protected GenericRepository(BookStoreDbContext context)
        => _context = context;

    private readonly BookStoreDbContext _context;

    public async Task<IQueryable<T>> FindAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
        => !trackChanges
                ? await Task.FromResult(_context.Set<T>().AsNoTracking())
                : await Task.FromResult(_context.Set<T>());

    public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default)
        => !trackChanges
                ? await Task.FromResult(_context.Set<T>().Where(expression).AsNoTracking())
                : await Task.FromResult(_context.Set<T>().Where(expression));

    public async Task<T> FirstAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default)
        => !trackChanges
                ? await _context.Set<T>().AsNoTracking().FirstAsync(expression, cancellationToken)
                : await _context.Set<T>().FirstAsync(expression, cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default)
        => !trackChanges
                ? await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken)
                : await _context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);


    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _context.Set<T>().AddRangeAsync(entities, cancellationToken);

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _context.Set<T>().CountAsync(cancellationToken);

    public async Task<IQueryable<T>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await Task.FromResult(_context.Set<T>().Skip((page - 1) * pageSize).Take(pageSize));
}
