using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository;
using Bookstore_WebAPI.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookStore_Tests.Repository_Tests
{
    public class BookRepository_Tests
    {
        public async Task<ApplicationContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                           .Options;

            var databaseContext = new ApplicationContext(options);

            databaseContext.Database.EnsureCreated();

            AddToDatabaseAsync(databaseContext);

            await databaseContext.SaveChangesAsync();

            return databaseContext;

            async Task AddToDatabaseAsync(ApplicationContext databaseContext)
            {
                    var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
                    var publishingHouse = new PublishingHouse { Id = 1, Name = "Publishing House A" };

                    var book = new Book { Id = 1, Name = "Book A", PublishingHouseId = publishingHouse.Id, PublishingHouse = publishingHouse };
                    var book2 = new Book { Id = 2, Name = "Book B", PublishingHouseId = publishingHouse.Id, PublishingHouse = publishingHouse };
                    var book3 = new Book { Id = 3, Name = "Book С", PublishingHouseId = publishingHouse.Id, PublishingHouse = publishingHouse };

                    var authorBooks = new AuthorBooks { Id = 1, Author = author, Book = book };
                    var authorBooks2 = new AuthorBooks { Id = 2, Author = author, Book = book2 };
                    var authorBooks3 = new AuthorBooks { Id = 3, Author = author, Book = book3 };

                    ICollection<AuthorBooks> collection = new List<AuthorBooks>() { authorBooks };
                    ICollection<AuthorBooks> collection2 = new List<AuthorBooks>() { authorBooks2 };
                    ICollection<AuthorBooks> collection3 = new List<AuthorBooks>() { authorBooks3 };

                    book.AuthorBooks = collection;
                    book2.AuthorBooks = collection2;
                    book3.AuthorBooks = collection3;

                    var authorPublishingHouse = new AuthorPublishingHouses { Id = 1, Author = author, PublishingHouse = publishingHouse };

                    await databaseContext.Books.AddRangeAsync(book, book2, book3);
                    await databaseContext.AuthorBooks.AddRangeAsync(authorBooks, authorBooks2, authorBooks3);
                    await databaseContext.PublishingHouses.AddAsync(publishingHouse);
                    await databaseContext.Authors.AddAsync(author);
                    await databaseContext.AuthorPublishingHouses.AddAsync(authorPublishingHouse);
            }
        }

        [Fact]
        public async void BookRepository_GetAsync_ReturnBook()
        {
            // Arrange
            var bookId = 1;
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);

            // Act
            var book = await bookRepository.GetAsync(bookId);

            // Assert 
            book.Should().NotBeNull();
            book.Should().BeOfType<Book>();
        }

        [Fact]
        public async void BookRepository_EntityExistsAsync_ReturnTrue()
        {
            // Arrange
            var bookId = 1;
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);

            // Act
            var isExists = await bookRepository.EntityExistsAsync(bookId);

            // Assert 
            isExists.Should().BeTrue();
        }

        [Fact]
        public async void BookRepository_GetAllAsync_ReturnBooks()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);

            // Act
            var books = await bookRepository.GetAllAsync();

            // Assert 
            books.Should().NotBeNull();
            books.Should().HaveCount(c => c <= 3).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async void BookRepository_CreateAsync_ReturnTrue()
        {
            // Arrange
            var book = new Book { Name = "test" };
            var author = new Author { FirstName = "test", LastName = "test" };
            var publishingHouse = new PublishingHouse { Name = "test" };

            var authorBooks = new AuthorBooks { Author = author, Book = book, };
            var authorPublishingHouses = new AuthorPublishingHouses { Author = author, PublishingHouse = publishingHouse };

            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);

            // Act
             var isCreated = await bookRepository.CreateBookAsync(book, authorBooks, authorPublishingHouses);
            var isExists = await bookRepository.EntityExistsAsync(4);
            var isNotExists = await bookRepository.EntityExistsAsync(5);

            // Assert 
            isCreated.Should().BeTrue();
            isExists.Should().BeTrue();
            isNotExists.Should().BeFalse();
        }

        [Fact]
        public async void BookRepository_DeleteAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);
            var newBook = await bookRepository.GetAsync(2);

            // Act
            var isDeleted = await bookRepository.DeleteAsync(newBook);
            var isExists = await bookRepository.EntityExistsAsync(2);

            // Assert 
            isDeleted.Should().BeTrue();
            isExists.Should().BeFalse();
        }

        [Fact]
        public async void BookRepository_UpdateAsync_ReturnTrueAndUpdatedBook()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);
            var book = await bookRepository.GetAsync(1);

            // Act
            book.Name = "test";
            var isCreated = await bookRepository.UpdateAsync(book);
            var updatedBook = await bookRepository.GetAsync(1);

            // Assert 
            isCreated.Should().BeTrue();
            updatedBook.Name.Should().Be("test");
            updatedBook.Should().BeOfType<Book>();
            updatedBook.Should().NotBeNull();
        }

        [Fact]
        public async void BookRepository_DeleteAllAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);
            var book1 = await bookRepository.GetAsync(1);
            var book2 = await bookRepository.GetAsync(2);
            List<Book> authors = new List<Book> { book1, book2 };

            // Act
            var isDeleted = await bookRepository.DeleteAllAsync(authors);

            // Assert 
            isDeleted.Should().BeTrue();
        }
    }
}