namespace Bookstore_WebAPI.Data.Models
{
    public class PublishingHouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BookId { get; set; }
        public Book? Book { get; set; }
        public ICollection<AuthorPublishingHouses> AuthorsPublishingHouses { get; set; }
    }
}
