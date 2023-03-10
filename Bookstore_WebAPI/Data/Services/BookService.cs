using AutoMapper;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services.Interfaces;

namespace Bookstore_WebAPI.Data.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IMapper mapper, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }
        public async Task<ICollection<BookDto>> GetAllMappingEntitiesAsync()
        {
            return _mapper.Map<ICollection<BookDto>>(await _bookRepository.GetAllAsync());
        }

        public async Task<BookDto> GetMappingEntityAsync(int id)
        {
            return _mapper.Map<BookDto>(await _bookRepository.GetAsync(id));
        }

        public async Task<bool> CreateMappingBookAsync(BookDto entityDto, int mainAuthorId)
        {
            var author = await _authorRepository.GetAsync(mainAuthorId);
            var book = MappingEntity(entityDto);

            var authorBooks = new AuthorBooks
            {
                Author = author,
                Book = book,
            };

            return await _bookRepository.CreateBookAsync(book, authorBooks);
        }

        public Book MappingEntity(BookDto entityDto)
        {
            return _mapper.Map<Book>(entityDto);
        }

        public Task<bool> UpdateMappingEntity(BookDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EntityExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
