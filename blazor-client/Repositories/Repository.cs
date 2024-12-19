using System.Net.Http.Json;

namespace blazor_client.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public Repository(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<List<T>>? GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<T>>(_baseUrl);
    }

    public async Task<T>? GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<T>($"{_baseUrl}/{id}");
    }

    public async Task<T> CreateAsync(T entity)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, entity);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task UpdateAsync(int id, T entity)
    {
        await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
    }
}