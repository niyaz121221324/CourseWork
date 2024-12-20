using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace blazor_client.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<Repository<T>>? _logger;

    public Repository(HttpClient httpClient, ILogger<Repository<T>>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = _httpClient.BaseAddress?.ToString() ?? throw new InvalidOperationException("Base address must be set for HttpClient.");
        _logger = logger;
    }

    public async Task<List<T>?> GetAllAsync()
    {
        try
        {
            _logger?.LogInformation("Fetching all entities from {Url}", _baseUrl);
            return await _httpClient.GetFromJsonAsync<List<T>>(_baseUrl);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching all entities from {Url}", _baseUrl);
            throw;
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            string url = $"{_baseUrl}/{id}";
            _logger?.LogInformation("Fetching entity by ID from {Url}", url);
            return await _httpClient.GetFromJsonAsync<T>(url);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching entity by ID {Id} from {Url}", id, _baseUrl);
            throw;
        }
    }

    public async Task<T> CreateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        try
        {
            _logger?.LogInformation("Creating a new entity at {Url}", _baseUrl);
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, entity);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to create entity. Status Code: {StatusCode}", response.StatusCode);
                response.EnsureSuccessStatusCode();
            }

            return await response.Content.ReadFromJsonAsync<T>()
                ?? throw new InvalidOperationException("Failed to deserialize the created entity.");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error creating entity at {Url}", _baseUrl);
            throw;
        }
    }

    public async Task UpdateAsync(int id, T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        try
        {
            string url = $"{_baseUrl}/{id}";
            _logger?.LogInformation("Updating entity with ID {Id} at {Url}", id, url);
            var response = await _httpClient.PutAsJsonAsync(url, entity);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to update entity with ID {Id}. Status Code: {StatusCode}", id, response.StatusCode);
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error updating entity with ID {Id} at {Url}", id, _baseUrl);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            string url = $"{_baseUrl}/{id}";
            _logger?.LogInformation("Deleting entity with ID {Id} from {Url}", id, url);
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to delete entity with ID {Id}. Status Code: {StatusCode}", id, response.StatusCode);
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error deleting entity with ID {Id} from {Url}", id, _baseUrl);
            throw;
        }
    }
}