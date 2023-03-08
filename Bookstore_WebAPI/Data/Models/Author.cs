namespace Bookstore_WebAPI.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Book>? Books { get; set; }
        public ICollection<PublishingHouse>? PublishingHouses { get; set; }
    }
}