using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bookstore_WebAPI.Data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationContext _context;

        public BookRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookAsync(Book entity, AuthorBooks authorBooks, AuthorPublishingHouses authorPublishingHouses)
        {
            await _context.AddAsync(authorBooks);
            await _context.AddAsync(entity);
            await _context.AddAsync(authorPublishingHouses);

            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Book entity)
        {
            _context.Remove(entity);
            return await SaveAsync();
        }

        public async Task<bool> EntityExistsAsync(int Id)
        {
            return await _context.Books.AnyAsync(b => b.Id == Id);
        }

        public async Task<ICollection<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetAsync(int Id)
        {
            var book = await _context.Books.Where(b => b.Id == Id).FirstOrDefaultAsync();
            var authorBooks = await _context.AuthorBooks.Where(ab => ab.BookId == Id).ToListAsync();
                var publishingHouse = await _context.AuthorPublishingHouses.Where(aph => aph.AuthorId == authorBooks[0].AuthorId).ToListAsync();

            book.AuthorBooks = authorBooks;
            book.PublishingHouse = await _context.PublishingHouses.Where(ph => ph.Id == publishingHouse[0].PublishingHouseId).FirstOrDefaultAsync();

            return book;
        }

        public async Task<bool> UpdateAsync(Book entity)
        {
            _context.Books.Update(entity);

            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<List<Book>> GetAllAuthorsBooks(int id)
        {
            var authorBooks = await _context.AuthorBooks.Where(i => i.AuthorId == id).ToListAsync();
            var books = new List<Book>();

            foreach(var book in authorBooks)
            {
                books.Add(await GetAsync(book.BookId));
            }

            return books;
        }

        public async Task<bool> DeleteAllAsync(List<Book> entities)
        {
            _context.RemoveRange(entities);
            return await SaveAsync();
        }

        public async Task<List<Book>> GetAllPublishingHouseBooks(int id)
        {
            var publishingHouseBooks = await _context.Books.Where(i => i.PublishingHouseId == id).ToListAsync();

            var books = new List<Book>();

            foreach (var book in publishingHouseBooks)
            {
                books.Add(await GetAsync(book.PublishingHouseId));
            }

            return books;
        }
    }
}
