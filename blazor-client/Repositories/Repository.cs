using System.Net.Http.Json;

namespace blazor_client.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<Repository<T>>? _logger;

    public Repository(HttpClient httpClient, ILogger<Repository<T>>? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = GetBaseUrlString(httpClient);
        _logger = logger;
    }

    private string GetBaseUrlString(HttpClient httpClient)
    {
        if (_httpClient.BaseAddress == null)
        {
            throw new InvalidOperationException("Base address must be set for HttpClient.");
        }

        return $"{httpClient.BaseAddress?.ToString()}/{typeof(T).Name}"; 
    }

    private async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
        else
        {
            _logger?.LogWarning("Request failed. Status Code: {StatusCode}", response.StatusCode);
            response.EnsureSuccessStatusCode();
            return default;  
        }
    }

    private void LogRequestInfo(string methodName, string url)
    {
        _logger?.LogInformation("{Method} request to {Url}", methodName, url);
    }

    private void LogError(string methodName, string url, Exception ex)
    {
        _logger?.LogError(ex, "Error during {Method} request to {Url}", methodName, url);
    }

    public async Task<List<T>?> GetAllAsync()
    {
        try
        {
            LogRequestInfo("GET", _baseUrl);
            var response = await _httpClient.GetAsync(_baseUrl);
            return await HandleResponseAsync<List<T>>(response);
        }
        catch (Exception ex)
        {
            LogError("GET", _baseUrl, ex);
            throw;
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            string url = $"{_baseUrl}/{id}";
            LogRequestInfo("GET by ID", url);
            var response = await _httpClient.GetAsync(url);
            return await HandleResponseAsync<T>(response);
        }
        catch (Exception ex)
        {
            LogError("GET by ID", $"{_baseUrl}/{id}", ex);
            throw;
        }
    }

    public async Task<T> CreateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        try
        {
            LogRequestInfo("POST", _baseUrl);
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, entity);
            return await HandleResponseAsync<T>(response)
                ?? throw new InvalidOperationException("Failed to deserialize the created entity.");
        }
        catch (Exception ex)
        {
            LogError("POST", _baseUrl, ex);
            throw;
        }
    }

    public async Task UpdateAsync(int id, T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        try
        {
            string url = $"{_baseUrl}/{id}";
            LogRequestInfo("PUT", url);
            var response = await _httpClient.PutAsJsonAsync(url, entity);
            await HandleResponseAsync<T>(response);  
        }
        catch (Exception ex)
        {
            LogError("PUT", $"{_baseUrl}/{id}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            string url = $"{_baseUrl}/{id}";
            LogRequestInfo("DELETE", url);
            var response = await _httpClient.DeleteAsync(url);
            await HandleResponseAsync<T>(response); 
        }
        catch (Exception ex)
        {
            LogError("DELETE", $"{_baseUrl}/{id}", ex);
            throw;
        }
    }
}