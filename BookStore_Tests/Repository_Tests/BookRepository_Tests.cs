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

            if (await databaseContext.Books.CountAsync() <= 0)
                await databaseContext.Books.AddRangeAsync(
                    new Book { Name = "1984"},
                    new Book { Name = "Fahrenheit 451" });

            await databaseContext.SaveChangesAsync();

            return databaseContext;
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
            books.Should().HaveCount(c => c <= 2).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async void BookRepository_CreateAsync_ReturnTrue()
        {
            // Arrange
            var book = new Book { Name = "test" };
            var author = new Author { FirstName = "test", LastName = "test" };
            var authorBooks = new AuthorBooks
            {
                Author = author,
                Book = book,
            };
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);

            // Act
            var isCreated = await bookRepository.CreateBookAsync(book, authorBooks);
            var isExists = await bookRepository.EntityExistsAsync(book.Id);

            // Assert 
            isCreated.Should().BeTrue();
            isExists.Should().BeTrue();
        }

        [Fact]
        public async void BookRepository_DeleteAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var bookRepository = new BookRepository(dbContext);
            var book = await bookRepository.GetAsync(1);

            // Act
            var isCreated = await bookRepository.DeleteAsync(book);
            var isExists = await bookRepository.EntityExistsAsync(book.Id);

            // Assert 
            isCreated.Should().BeTrue();
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
    }
}