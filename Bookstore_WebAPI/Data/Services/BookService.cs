using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services.Interfaces;
using System.Net;

namespace Bookstore_WebAPI.Data.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublishingHouseRepository _publishingHouseRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper, IAuthorRepository authorRepository, IPublishingHouseRepository publishingHouseRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _publishingHouseRepository = publishingHouseRepository;
        }
        public async Task<ICollection<BookDto>> GetAllMappingEntitiesAsync()
        {
            return _mapper.Map<ICollection<BookDto>>(await _bookRepository.GetAllAsync());
        }

        public async Task<BookDto> GetMappingEntityAsync(int id)
        {
            return _mapper.Map<BookDto>(await _bookRepository.GetAsync(id));
        }

        public async Task<bool> CreateMappingBookAsync(BookDto entityDto, int mainAuthorId, int publishingHouseId)
        {
            var author = await _authorRepository.GetAsync(mainAuthorId);
            var publishingHouse = await _publishingHouseRepository.GetAsync(publishingHouseId);
            var book = MappingEntity(entityDto);
            book.PublishingHouse = publishingHouse;

            var authorBooks = new AuthorBooks
            {
                Author = author,
                Book = book,
            };

            var authorPublishingHouses = new AuthorPublishingHouses
            {
                Author = author,
                PublishingHouse = publishingHouse,
            };

            return await _bookRepository.CreateBookAsync(book, authorBooks, authorPublishingHouses);
        }

        public Book MappingEntity(BookDto entityDto)
        {
            return _mapper.Map<Book>(entityDto);
        }

        public async Task<bool> UpdateMappingEntityAsync(BookDto entity)
        {
            var updatedBook = MappingEntity(entity);
            var book = await _bookRepository.GetAsync(updatedBook.Id);

            book.Name = updatedBook.Name;

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<bool> DeleteEntityAsync(int id)
        {
            var book = await _bookRepository.GetAsync(id);

            return await _bookRepository.DeleteAsync(book);
        }

        public async Task<bool> EntityExistsAsync(int id)
        {
            return await _bookRepository.EntityExistsAsync(id);
        }

        public async Task<bool> CheckDependentEntitiesExist(int mainAuthorId, int publishingHouseId)
        {
           var author = await _authorRepository.EntityExistsAsync(mainAuthorId);
            var publishingHouse = await _publishingHouseRepository.EntityExistsAsync(publishingHouseId);
                if (author && publishingHouse)
                    return true;

                else
                    return false;
        }
    }
}
