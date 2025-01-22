using System.Linq.Expressions;

namespace CloudTrack.Competitions.Domain.Common;

public interface IRepository<TEntity, TId>
    where TEntity : IAggregateRoot
{
    Task<TId> CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetAsync(TId id, params Expression<Func<TEntity, object>>[] includes);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TId id);
}
