using System.Linq.Expressions;
using FreightFlow.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FreightFlow.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        T? entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
        }
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var keyPropertyName = await GetKeyPropertyName();
        if (string.IsNullOrEmpty(keyPropertyName))
        {
            throw new InvalidOperationException("Key property name cannot be null or empty.");
        }

        var query = _dbSet.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = query.OrderByDescending(e => EF.Property<object>(e, keyPropertyName));

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>?> GetAllAsync(params string[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id, params string[] includeProperties)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be greater than zero.", nameof(id));
        }

        var keyName = await GetKeyPropertyName();

        IQueryable<T> query = _dbSet;

        if (includeProperties != null && includeProperties.Length > 0)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.FirstOrDefaultAsync(entity => EF.Property<int>(entity, keyName) == id);
    }

    public Task<string> GetKeyPropertyName() 
    {
        var keyProperty = _dbSet.EntityType.FindPrimaryKey()?.Properties.FirstOrDefault();
        if (keyProperty == null)
        {
            throw new InvalidOperationException("The entity does not have a primary key defined.");
        }

        return Task.FromResult(keyProperty.Name);
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public async Task Update(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbContext.Entry(entity).State = EntityState.Modified;

        var modifiedProperties = _dbContext.Entry(entity).Properties
            .Where(p => p.IsModified)
            .Select(p => p.Metadata.Name)
            .ToList();

        await _dbContext.SaveChangesAsync();
    }
}