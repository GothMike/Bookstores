using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_WebAPI.Data.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationContext _context;
        public AuthorRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAsync(int Id)
        {
            return await _context.Authors.Where(a => a.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<bool> EntityExistsAsync(int Id)
        { 
           return await _context.Authors.AnyAsync(a => a.Id == Id); 
        }

        public async Task<bool> CreateAuthorAsync(Author entity)
        {
            await _context.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Author entity)
        {
            _context.Authors.Remove(entity);
            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(Author entity)
        {
            _context.Authors.Update(entity);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
