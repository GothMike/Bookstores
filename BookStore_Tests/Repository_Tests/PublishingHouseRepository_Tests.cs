
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository;
using Bookstore_WebAPI.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Tests.Repository_Tests
{
    public class PublishingHouseRepository_Tests
    {
        public async Task<ApplicationContext> GetDatabaseContext()
        {

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                           .Options;

            var databaseContext = new ApplicationContext(options);

            databaseContext.Database.EnsureCreated();

            if (await databaseContext.PublishingHouses.CountAsync() <= 0)
                await databaseContext.PublishingHouses.AddRangeAsync(
                    new PublishingHouse { Name = "Ballantine Books" },
                    new PublishingHouse { Name = "Secker & Warburg" });

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Fact]
        public async void PublishingHouseRepository_GetAsync_ReturnPublishingHouse()
        {
            // Arrange
            var publishingHouseId = 1;
            var dbContext = await GetDatabaseContext();
            var publishingHouseRepository = new PublishingHouseRepository(dbContext);

            // Act
            var publishingHouse = await publishingHouseRepository.GetAsync(publishingHouseId);

            // Assert 
            publishingHouse.Should().NotBeNull();
            publishingHouse.Should().BeOfType<PublishingHouse>();
        }

        [Fact]
        public async void PublishingHousetRepository_EntityExistsAsync_ReturnTrue()
        {
            // Arrange
            var publishingHouseId = 1;
            var dbContext = await GetDatabaseContext();
            var publishingHouseRepository = new PublishingHouseRepository(dbContext);

            // Act
            var publishingHouse = await publishingHouseRepository.EntityExistsAsync(publishingHouseId);

            // Assert 
            publishingHouse.Should().BeTrue();
        }

        [Fact]
        public async void PublishingHouseRepository_GetAllAsync_ReturnPublishingHouses()
        {
            // Arrange
             var dbContext = await GetDatabaseContext();
             var publishingHouseRepository = new PublishingHouseRepository(dbContext);

            // Act
            var publishingHouses = await publishingHouseRepository.GetAllAsync();

            // Assert 
            publishingHouses.Should().NotBeNull();
            publishingHouses.Should().HaveCount(c => c <= 2).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async void PublishingHouseRepository_CreateAsync_ReturnTrue()
        {
            // Arrange
            var publishingHouse = new PublishingHouse { Name = "test" };
            var dbContext = await GetDatabaseContext();
            var publishingHouseRepository = new PublishingHouseRepository(dbContext);

            // Act
            var isCreated = await publishingHouseRepository.CreatePublishingHouseAsync(publishingHouse);
            var isExists = await publishingHouseRepository.EntityExistsAsync(publishingHouse.Id);

            // Assert 
            isCreated.Should().BeTrue();
            isExists.Should().BeTrue();
        }

        [Fact]
        public async void PublishingHouseRepository_DeleteAsync_ReturnTrue()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var publishingHouseRepository = new PublishingHouseRepository(dbContext);
            var publishingHouse = await publishingHouseRepository.GetAsync(1);

            // Act
            var isCreated = await publishingHouseRepository.DeleteAsync(publishingHouse);
            var isExists = await publishingHouseRepository.EntityExistsAsync(publishingHouse.Id);

            // Assert 
            isCreated.Should().BeTrue();
            isExists.Should().BeFalse();
        }

        [Fact]
        public async void PublishingHouseRepository_UpdateAsync_ReturnTrueAndUpdatedPublishingHouse()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var publishingHouseRepository = new PublishingHouseRepository(dbContext);
            var publishingHouse = await publishingHouseRepository.GetAsync(1);

            // Act
            publishingHouse.Name = "test";
            var isCreated = await publishingHouseRepository.UpdateAsync(publishingHouse);
            var updatedPublishingHouse = await publishingHouseRepository.GetAsync(1);

            // Assert 
            isCreated.Should().BeTrue();
            updatedPublishingHouse.Name.Should().Be("test");
            updatedPublishingHouse.Should().BeOfType<PublishingHouse>();
            updatedPublishingHouse.Should().NotBeNull();
        }
    }
}