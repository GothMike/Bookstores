namespace Bookstore_WebAPI.Data.Models
{
    public class AuthorBooks
    {
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
