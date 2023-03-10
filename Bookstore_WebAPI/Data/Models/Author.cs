using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore_WebAPI.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AuthorPublishingHouses> AuthorPublishingHouses { get; set; }
        public ICollection<AuthorBooks> AuthorBooks { get; set; }
    }
}
