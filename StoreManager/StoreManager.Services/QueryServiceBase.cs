using System.Linq.Expressions;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public abstract class QueryServiceBase<TEntity, TRepository> : IQueryService<TEntity>
    where TEntity : class, IEntity
    where TRepository : IRepository<TEntity>
{
    private readonly IRepository<TEntity> _repository;

    public QueryServiceBase(TRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public virtual TEntity Get(int id) => _repository
	    .Set(x => x.Id == id && !x.IsDeleted)
	    .Single();

    public virtual IQueryable<TEntity> Set(Expression<Func<TEntity, bool>> predicate) => _repository.Set(predicate);

    public virtual IQueryable<TEntity> Set() => _repository.Set();
}
