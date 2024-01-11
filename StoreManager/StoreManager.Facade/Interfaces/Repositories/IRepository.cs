using System.Linq.Expressions;

namespace StoreManager.Facade.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Get(params object?[]? keyValues);
    IQueryable<TEntity> Set(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> Set();

    void Insert(TEntity entity);
    void InsertRange(ICollection<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(params object?[]? keyValues);
}