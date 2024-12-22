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
        await SaveChangesAsync("An error occurred while creating the new entity.");
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync("An error occurred while deleting the entity.");
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

    public async Task Update(int id, T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
        }

        var existingEntity = await GetByIdAsync(id);
        if (existingEntity == null)
        {
            throw new InvalidOperationException($"Entity with id {id} not found.");
        }
        
        var keyPropertyName = await GetKeyPropertyName();
        var trackedEntity = _dbSet.Local.FirstOrDefault(e => e.GetType().GetProperty(keyPropertyName)?.GetValue(e)?.Equals(id) == true);

        if (trackedEntity != null)
        {
            _dbContext.Entry(trackedEntity).CurrentValues.SetValues(entity);
        }
        else
        {
            AttachEntityAndMarkModified(entity);
        }

        await SaveChangesAsync("An error occurred while updating the entity.");
    }

    private void AttachEntityAndMarkModified(T entity)
    {
        var entry = _dbContext.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    private async Task SaveChangesAsync(string errorMessage)
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the entity.", ex);
        }
    }
}