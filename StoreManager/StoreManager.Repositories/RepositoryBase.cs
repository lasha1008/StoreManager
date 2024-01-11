using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

internal abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly StoreManagerDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected RepositoryBase(StoreManagerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public T Get(params object?[]? keyValues) =>
        _dbSet.Find(keyValues) ?? throw new KeyNotFoundException($"Record with key {keyValues} not found");

    public IQueryable<T> Set(Expression<Func<T, bool>> predicate) =>
        _dbSet.Where(predicate);

    public IQueryable<T> Set() =>
        _dbSet;

    public void Insert(T entity) =>
        _dbSet.Add(entity);

    public void InsertRange(ICollection<T> entities) =>
        _dbSet.AddRange(entities);

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(params object?[]? keyValues) =>
        Delete(Get(keyValues));

    public void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }
}
