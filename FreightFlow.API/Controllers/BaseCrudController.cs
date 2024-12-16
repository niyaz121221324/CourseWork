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
            var keyPropertyName = await _repository.GetKeyPropertyName();
            var idProperty = typeof(T).GetProperty(keyPropertyName);
            if (idProperty == null)
            {
                return BadRequest("Entity does not have an 'Id' property.");
            }

            var id = (int?)idProperty.GetValue(entity);

            await _repository.CreateAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                entity
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, T entity)
        {
            if (id <= 0 || entity == null || !id.Equals(typeof(T).GetProperty("Id")?.GetValue(entity)))
            {
                return BadRequest("Invalid data provided.");
            }

            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return NotFound(); 
            }

            try
            {
                await _repository.Update(entity);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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