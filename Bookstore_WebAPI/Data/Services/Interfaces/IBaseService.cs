namespace Bookstore_WebAPI.Data.Services
{
    public interface IBaseService<T, K>
    {
        K MappingEntity(T entity);
        Task<ICollection<T>> GetAllMappingEntitiesAsync();
        Task<T> GetMappingEntityAsync(int id);
        Task<bool> UpdateMappingEntity(T entity);  
        Task<bool> DeleteEntityAsync(int id);
        Task<bool> EntityExistsAsync(int id);
    }
}
