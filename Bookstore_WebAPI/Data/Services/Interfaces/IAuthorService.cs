using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;

namespace Bookstore_WebAPI.Data.Services.Interfaces
{
    public interface IAuthorService : IBaseService<AuthorDto, Author>
    {
        Task<bool> CreateMappingAuthorAsync(AuthorDto entityDto);
    }
}
