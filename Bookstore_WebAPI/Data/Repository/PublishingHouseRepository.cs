using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_WebAPI.Data.Repository
{
    public class PublishingHouseRepository : IPublishRepository
    {
        private readonly ApplicationContext _context;

        public PublishingHouseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(PublishingHouse entity)
        {
            await _context.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(PublishingHouse entity)
        {
            _context.Remove(entity);
            return await SaveAsync();
        }

        public async Task<bool> EntityExistsAsync(int Id)
        {
            return await _context.PublishingHouses.AnyAsync(ph => ph.Id == Id);
        }

        public async Task<ICollection<PublishingHouse>> GetAllAsync()
        {
            return await _context.PublishingHouses.ToListAsync();
        }

        public async Task<PublishingHouse> GetAsync(int Id)
        {
            return await _context.PublishingHouses.Where(ph => ph.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(PublishingHouse entity)
        {
            _context.Update(entity);
            return  await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}