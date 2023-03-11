using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;

namespace Bookstore_WebAPI.Data.Repository.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<bool> CreateBookAsync(Book entity, AuthorBooks authorBooks, AuthorPublishingHouses authorPublishingHouses);
        Task<List<Book>> GetAllAuthorsBooks(int id);
        Task<List<Book>> GetAllPublishingHouseBooks(int id);
    }
}
