namespace Bookstore_WebAPI.Data.Models
{
    public class AuthorPublishingHouses
    {
        public int AuthorId { get; set; }
        public int PublishingHouseId { get; set; }
        public Author Author { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
    }
}
