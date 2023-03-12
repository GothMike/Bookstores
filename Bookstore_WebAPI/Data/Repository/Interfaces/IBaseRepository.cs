﻿using System.Collections.Generic;

namespace Bookstore_WebAPI.Data.Repository.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> GetAsync(int Id);
        Task<ICollection<T>> GetAllAsync();
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAllAsync(List<T> entities);
        Task<bool> UpdateAsync (T entity);
        Task<bool> EntityExistsAsync(int Id);
        Task<bool> SaveAsync();
    }
}
