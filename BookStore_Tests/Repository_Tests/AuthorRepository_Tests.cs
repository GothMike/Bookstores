using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository;
using Bookstore_WebAPI.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Tests.Repository_Tests
{
    public class AuthorRepository_Tests
    {
        public async Task<ApplicationContext> GetDatabaseContext()
        {

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                           .Options;

            var databaseContext = new ApplicationContext(options);

            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Authors.CountAsync() <= 0)
                await databaseContext.Authors.AddRangeAsync(
                     new Author { FirstName = "Ray", LastName = "Bradbury" },
                     new Author { FirstName = "George", LastName = "Orwell" });

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Fact]
        public async void AuthorRepository_GetAsync_ReturnAuthor()
        {
            // Arrange
            var authorId = 1;
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);

            // Act
            var author = await authorRepository.GetAsync(authorId);

            // Assert 
            author.Should().NotBeNull();
            author.Should().BeOfType<Author>();
        }

        [Fact]
        public async void AuthorRepository_EntityExistsAsync_ReturnTrue()
        {
            // Arrange
            var authorId = 1;
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);

            // Act
            var author = await authorRepository.EntityExistsAsync(authorId);

            // Assert 
            author.Should().BeTrue();
        }

        [Fact]
        public async void AuthorRepository_GetAllAsync_ReturnAuthors()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);

            // Act
            var authors = await authorRepository.GetAllAsync();

            // Assert 
            authors.Should().NotBeNull();
            authors.Should().HaveCount(c => c <= 2).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async void AuthorRepository_CreateAsync_ReturnTrue()
        {
            // Arrange
            var author = new Author { FirstName = "test" , LastName = "test"};
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);

            // Act
            var isCreated = await authorRepository.CreateAuthorAsync(author);
            var isExists = await authorRepository.EntityExistsAsync(author.Id);

            // Assert 
            isCreated.Should().BeTrue();
            isExists.Should().BeTrue();
        }

        [Fact]
        public async void AuthorRepository_DeleteAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);
            var author = await authorRepository.GetAsync(1);

            // Act
            var isDeleted = await authorRepository.DeleteAsync(author);
            var isExists = await authorRepository.EntityExistsAsync(author.Id);

            // Assert 
            isDeleted.Should().BeTrue();
            isExists.Should().BeFalse();
        }

        [Fact]
        public async void AuthorRepository_UpdateAsync_ReturnTrueAndUpdatedAuthor()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);
            var author = await authorRepository.GetAsync(1);

            // Act
            author.FirstName = "test";
            author.LastName = "test";
            var isCreated = await authorRepository.UpdateAsync(author);
            var updatedAuthor = await authorRepository.GetAsync(1);

            // Assert 
            isCreated.Should().BeTrue();
            updatedAuthor.FirstName.Should().Be("test");
            updatedAuthor.LastName.Should().Be("test");
            updatedAuthor.Should().BeOfType<Author>();
            updatedAuthor.Should().NotBeNull();
        }

        [Fact]
        public async void AuthorRepository_DeleteAllAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var authorRepository = new AuthorRepository(dbContext);
            var author1 = await authorRepository.GetAsync(1);
            var author2 = await authorRepository.GetAsync(2);
            List<Author> authors = new List<Author> { author1, author2 };

            // Act
           var isDeleted = await authorRepository.DeleteAllAsync(authors);

            // Assert 
            isDeleted.Should().BeTrue();
        }
    }
}