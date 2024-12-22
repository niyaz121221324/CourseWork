using System.Reflection;
using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FreightFlow.API.Controllers;

public abstract class BaseCrudController<T> : BaseApiController where T : class
{
    private readonly IRepository<T> _repository;

    protected BaseCrudController(ApplicationDbContext dbContext)
    {
        _repository = new Repository<T>(dbContext);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var entities = await _repository.GetAllAsync();
        if (entities == null || !entities.Any())
        {
            return NoContent(); 
        }

        return Ok(entities); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid ID.");
        }

        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity); 
    }

    [HttpPost]
    public async Task<ActionResult<T>> Create(T entity)
    {
        if (entity == null)
        {
            return BadRequest("Entity cannot be null.");
        }

        try
        {
            var keyPropertyName = await GetKeyPropertyNameAsync();
            var idProperty = ValidateKeyProperty(keyPropertyName);

            ValidateEntityIdForCreation(entity, idProperty);

            await _repository.CreateAsync(entity);

            var lastInsertedValue = await _repository.LastOrDefaultAsync();

            return Ok(lastInsertedValue);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private async Task<string> GetKeyPropertyNameAsync()
    {
        var keyPropertyName = await _repository.GetKeyPropertyName();
        if (string.IsNullOrEmpty(keyPropertyName))
        {
            throw new InvalidOperationException("Key property name cannot be determined.");
        }
        return keyPropertyName;
    }

    private PropertyInfo ValidateKeyProperty(string keyPropertyName)
    {
        var idProperty = typeof(T).GetProperty(keyPropertyName);
        if (idProperty == null)
        {
            throw new InvalidOperationException($"Entity does not have a key property '{keyPropertyName}'.");
        }
        return idProperty;
    }

    private void ValidateEntityIdForCreation(T entity, PropertyInfo idProperty)
    {
        var idValue = idProperty.GetValue(entity);
        if (idValue != null && Convert.ToInt32(idValue) > 0)
        {
            throw new InvalidOperationException("Entity ID should not be set for creation.");
        }
    }

    private ActionResult<T> HandleException(Exception ex)
    {
        return ex switch
        {
            InvalidOperationException => BadRequest(ex.Message),
            _ => StatusCode(500, $"Internal server error: {ex.Message}")
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, T entity)
    {
        var validationResult = await ValidateEntityAsync(id, entity);
        if (validationResult is not OkResult)
        {
            return validationResult;
        }

        try
        {
            await _repository.Update(id, entity);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private async Task<IActionResult> ValidateEntityAsync(int id, T entity)
    {
        if (entity == null)
        {
            return BadRequest("Entity cannot be null.");
        }

        var keyPropertyName = await _repository.GetKeyPropertyName();
    
        if (string.IsNullOrEmpty(keyPropertyName))
        {
            return BadRequest("Entity does not have a valid key property.");
        }

        var entityId = typeof(T).GetProperty(keyPropertyName)?.GetValue(entity);

        if (entityId == null || !id.Equals(entityId))
        {
            return BadRequest($"Entity Id ({entityId}) does not match the provided id ({id}).");
        }

        return Ok(); 
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid ID.");
        }

        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound(); 
        }

        try
        {
            await _repository.Delete(entity);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}