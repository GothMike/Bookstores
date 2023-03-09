using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_WebAPI.Data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationContext _context;

        public BookRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Book entity)
        {
            await _context.AddAsync(entity);
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
            return await _context.Books.Where(b => b.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Book entity)
        {
            _context.Update(entity);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}