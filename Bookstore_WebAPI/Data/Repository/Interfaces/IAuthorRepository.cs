using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Bookstore_WebAPI.Data.Repository.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<bool> CreateAuthorAsync(Author entity);
    }
}
