using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore_WebAPI.Data.Models
{
    public class PublishingHouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
        public ICollection<AuthorPublishingHouses>? AuthorsPublishingHouses { get; set; }
    }
}
