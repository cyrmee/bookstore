using System.Linq.Expressions;

namespace Infrastructure.Repositories.Interfaces;

public interface IGenericRepository<T>
{
    Task<IQueryable<T>> FindAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<T> FirstAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<T>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
