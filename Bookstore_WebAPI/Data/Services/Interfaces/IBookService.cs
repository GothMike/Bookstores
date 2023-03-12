using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;

namespace Bookstore_WebAPI.Data.Services.Interfaces
{
    public interface IBookService : IBaseService<BookDto, Book>
    {
        Task<bool> CreateMappingBookAsync(BookDto entityDto, int mainAuthorId, int publishingHouseId);
        Task<bool> CheckDependentEntitiesExist(int mainAuthorId, int publishingHouseId);
    }
}
