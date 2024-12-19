namespace blazor_client.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>>? GetAllAsync();

    Task<T>? GetByIdAsync(int id);

    Task<T> CreateAsync(T entity);

    Task UpdateAsync(int id, T entity);
    
    Task DeleteAsync(int id);
}