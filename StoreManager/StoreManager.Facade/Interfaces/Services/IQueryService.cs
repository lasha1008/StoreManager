using System.Linq.Expressions;

namespace StoreManager.Facade.Interfaces.Services;

public interface IQueryService<TEntity>
{
    TEntity Get(int id);
    IQueryable<TEntity> Set(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> Set();
}
