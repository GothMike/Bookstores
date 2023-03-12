using AutoMapper;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services;
using FakeItEasy;
using FluentAssertions;


namespace Bookstore_Tests.Services_Tests
{
    public class AuthorService_Tests
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public AuthorService_Tests()
        {
            _authorRepository = A.Fake<IAuthorRepository>();
            _bookRepository = A.Fake<IBookRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async void AuthorService_GetAllMappingEntitiesAsync_ReturnAuthorsDto()
        {
            // Arrange
            var authorList = new List<Author> { new Author { Id = 1, FirstName = "John", LastName = "Doe" }, new Author { Id = 2, FirstName = "Jane", LastName = "Doe" } };
            A.CallTo(() => _authorRepository.GetAllAsync()).Returns(authorList);
            var authorDtoList = new List<AuthorDto> { new AuthorDto { Id = 1, FirstName = "John", LastName = "Doe" }, new AuthorDto { Id = 2, FirstName = "Jane", LastName = "Doe" } };
            A.CallTo(() => _mapper.Map<ICollection<AuthorDto>>(authorList)).Returns(authorDtoList);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.GetAllMappingEntitiesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async void AuthorService_GetMappingEntityAsync_ReturnAthorDto()
        {
            // Arrange
            var authorId = 3;
            var author = new Author { Id = authorId, FirstName = "John", LastName = "Doe" };
            var authorDto = new AuthorDto { Id = authorId, FirstName = "John", LastName = "Doe" };
            A.CallTo(() => _authorRepository.GetAsync(authorId)).Returns(author);
            A.CallTo(() => _mapper.Map<AuthorDto>(author)).Returns(authorDto);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.GetMappingEntityAsync(3);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AuthorDto>();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
        }

        [Fact]
        public async void AuthorService_CreateMappingAuthorAsync_ReturnTrue()
        {
            // Arrange
            var authorDto = new AuthorDto { FirstName = "John", LastName = "Doe" };
            var author = new Author { FirstName = "John", LastName = "Doe" };
            A.CallTo(() => _mapper.Map<Author>(authorDto)).Returns(author);
            A.CallTo(() => _authorRepository.CreateAuthorAsync(author)).Returns(true);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.CreateMappingAuthorAsync(authorDto);

            // Assert
            result.Should().BeTrue();
            authorDto.FirstName.Should().BeSameAs(author.FirstName);
            authorDto.LastName.Should().BeSameAs(author.LastName);
        }

        [Fact]
        public void AuthorService_MappingEntity_ReturnAuthorDto()
        {
            // Arrange
            var authorDto = new AuthorDto { FirstName = "John", LastName = "Doe" };
            var author = new Author { FirstName = "John", LastName = "Doe" };
            A.CallTo(() => _mapper.Map<Author>(authorDto)).Returns(author);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = authorService.MappingEntity(authorDto);

            // Assert
            result.Should().BeSameAs(author);
            authorDto.FirstName.Should().BeSameAs(author.FirstName);
            authorDto.LastName.Should().BeSameAs(author.LastName);
        }

        [Fact]
        public async void AuthorService_UpdateMappingEntity_ReturnTrue()
        {
            // Arrange
            var authorDto = new AuthorDto { FirstName = "newJohn", LastName = "newDoe" };
            var author = new Author();
            A.CallTo(() => _mapper.Map<Author>(authorDto)).Returns(author);
            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            A.CallTo(() => _authorRepository.UpdateAsync(author)).Returns(true);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.UpdateMappingEntityAsync(authorDto);

            // Assert
            result.Should().BeTrue();
            authorDto.FirstName.Should().BeSameAs(author.FirstName);
            authorDto.LastName.Should().BeSameAs(author.LastName);
        }

        [Fact]
        public async void AuthorService_DeleteEntityAsync_ReturnTrue()
        {
            // Arrange
            var authorId = 1;
            var author = new Author { Id = authorId };
            A.CallTo(() => _authorRepository.GetAsync(authorId)).Returns(author);

            var authorBooks = new List<Book>
             {
        new Book { Id = 1, Name = "Book 1", AuthorBooks = new List<AuthorBooks> { new AuthorBooks { AuthorId = authorId, BookId = 1 } } },
        new Book { Id = 2, Name = "Book 2", AuthorBooks = new List<AuthorBooks> { new AuthorBooks { AuthorId = authorId, BookId = 2 } } }
             };
            A.CallTo(() => _bookRepository.GetAllAuthorsBooks(authorId)).Returns(authorBooks);

            A.CallTo(() => _bookRepository.DeleteAllAsync(authorBooks)).Returns(true);
            A.CallTo(() => _authorRepository.DeleteAsync(author)).Returns(true);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.DeleteEntityAsync(authorId);

            // Assert
            A.CallTo(() => _authorRepository.GetAsync(authorId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _bookRepository.GetAllAuthorsBooks(authorId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _bookRepository.DeleteAllAsync(authorBooks)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authorRepository.DeleteAsync(author)).MustHaveHappenedOnceExactly();
            Assert.True(result);
        }

        [Fact]
        public async Task AuthorService_GetAllMappingAuthorBooks_ShouldReturnMappedBookDtoList()
        {
            // Arrange
            int authorId = 1;

            var expectedBooks = new List<Book>()
                 {
        new Book { Id = 1, Name = "Book A" },
        new Book { Id = 2, Name = "Book B" }
                 };

            A.CallTo(() => _authorRepository.GetAsync(authorId)).Returns(new Author { Id = authorId });
            A.CallTo(() => _bookRepository.GetAllAuthorsBooks(authorId)).Returns(expectedBooks);

            var expectedBookDtos = new List<BookDto>()
                 {
        new BookDto { Id = 1, Name = "Book A" },
        new BookDto { Id = 2, Name = "Book B" }
                 };

            A.CallTo(() => _mapper.Map<ICollection<BookDto>>(expectedBooks)).Returns(expectedBookDtos);
            var authorService = new AuthorService(_authorRepository, _bookRepository, _mapper);

            // Act
            var result = await authorService.GetAllMappingAuthorBooks(authorId);

            // Assert
            result.Should().BeEquivalentTo(expectedBookDtos);
        }
    }
}
