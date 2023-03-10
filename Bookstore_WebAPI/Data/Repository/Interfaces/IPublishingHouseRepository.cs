using Bookstore_WebAPI.Data.Models;
using System.Diagnostics.Metrics;

namespace Bookstore_WebAPI.Data.Repository.Interfaces
{
    public interface IPublishingHouseRepository : IBaseRepository<PublishingHouse>
    {
        Task<bool> CreateAsync(PublishingHouse entity);
    }
}
