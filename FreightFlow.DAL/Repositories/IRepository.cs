using System.Linq.Expressions;

namespace FreightFlow.DAL.Repositories;

/// <summary>
/// Общий интерфейс репозитория для управления сущностями типа <typeparamref name="T"/>.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Асинхронно получает коллекцию сущностей <typeparamref name="T"/> из базы данных с возможностью фильтрации, сортировки и включения связанных сущностей.
    /// </summary>
    /// <param name="includeProperties">Массив строк с названиями связанных сущностей, которые нужно включить</param>
    /// <returns>Задача, возвращающая коллекцию сущностей <typeparamref name="T"/></returns>
    Task<IEnumerable<T>?> GetAllAsync(params string[] includeProperties);

    /// <summary>
    /// Асинхронно получает сущность <typeparamref name="T"/> из базы данных по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="includeProperties">Массив строк с названиями связанных сущностей, которые нужно включить</param>
    /// <returns>Задача, возвращающая сущность <typeparamref name="T"/> с указанным идентификатором</returns>
    Task<T?> GetByIdAsync(int id, params string[] includeProperties);

    /// <summary>
    /// Асинхронно создаёт новую сущность <typeparamref name="T"/> в базе данных.
    /// </summary>
    /// <param name="entity">Создаваемая сущность</param>
    /// <returns>Задача, представляющая создание сущности <typeparamref name="T"/></returns>
    Task CreateAsync(T entity);

    /// <summary>
    /// Асинхронно удаляет сущность <typeparamref name="T"/> из базы данных по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности</param>
    /// <returns>Задача, представляющая удаление сущности <typeparamref name="T"/></returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Асинхронно удаляет сущность <typeparamref name="T"/> из базы данных.
    /// </summary>
    /// <param name="entity">Удаляемая сущность</param>
    /// <returns>Задача, представляющая удаление сущности <typeparamref name="T"/></returns>
    Task Delete(T entity);

    /// <summary>
    /// Асинхронно обновляет сущность <typeparamref name="T"/> в базе данных.
    /// </summary>
    /// <param name="entity">Обновляемая сущность</param>
    /// <returns>Задача, представляющая обновление сущности <typeparamref name="T"/></returns>
    Task Update(T entity);

    /// <summary>
    /// Асинхронно возвращает первую сущность, удовлетворяющую указанному условию, или null, если сущность не найдена.
    /// </summary>
    /// <param name="predicate">Условие для фильтрации сущностей</param>
    /// <returns>Задача, возвращающая первую сущность <typeparamref name="T"/> или null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Асинхронно возвращает последнюю сущность, удовлетворяющую указанному условию, или null, если сущность не найдена.
    /// </summary>
    /// <param name="predicate">Условие для фильтрации сущностей</param>
    /// <returns>Задача, возвращающая последнюю сущность <typeparamref name="T"/> или null</returns>
    Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// Асинхронно возвращает единственную сущность, удовлетворяющую указанному условию, или null, если сущность не найдена.
    /// </summary>
    /// <param name="predicate">Условие для фильтрации сущностей</param>
    /// <returns>Задача, возвращающая единственную сущность <typeparamref name="T"/> или null</returns>
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Асинхронно проверяет, существует ли хотя бы одна сущность, удовлетворяющая указанному условию.
    /// </summary>
    /// <param name="predicate">Условие для фильтрации сущностей</param>
    /// <returns>Задача, возвращающая true, если сущность найдена, иначе false</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Асинхронно извлекает имя ключевого свойства сущности.
    /// </summary>
    /// <returns>
    /// Задача, представляющая асинхронную операцию. Результат задачи содержит имя ключевого свойства в виде строки.
    /// </returns>
    Task<string> GetKeyPropertyName();
}