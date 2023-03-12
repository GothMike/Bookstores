using AutoMapper;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services.Interfaces;

namespace Bookstore_WebAPI.Data.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<AuthorDto>> GetAllMappingEntitiesAsync()
        {
            return _mapper.Map<ICollection<AuthorDto>>(await _authorRepository.GetAllAsync());
        }

        public async Task<AuthorDto> GetMappingEntityAsync(int id)
        {
            return _mapper.Map<AuthorDto>(await _authorRepository.GetAsync(id));
        }

        public async Task<bool> CreateMappingAuthorAsync(AuthorDto entityDto)
        {
            return await _authorRepository.CreateAuthorAsync(MappingEntity(entityDto));
        }

        public Author MappingEntity(AuthorDto entityDto)
        {
            return _mapper.Map<Author>(entityDto);
        }

        public async Task<bool> UpdateMappingEntityAsync(AuthorDto entity)
        {
            return await _authorRepository.UpdateAsync(MappingEntity(entity));
        }

        public async Task<bool> DeleteEntityAsync(int id)
        {
            var author = await _authorRepository.GetAsync(id);
            var authorBooks = await _bookRepository.GetAllAuthorsBooks(author.Id);

            await _bookRepository.DeleteAllAsync(authorBooks);

            return await _authorRepository.DeleteAsync(author);
        }

        public Task<bool> EntityExistsAsync(int id)
        {
            return _authorRepository.EntityExistsAsync(id);
        }

        public async Task<ICollection<BookDto>> GetAllMappingAuthorBooks(int id)
        {
         var author = await _authorRepository.GetAsync(id);
         return _mapper.Map<ICollection<BookDto>>(await _bookRepository.GetAllAuthorsBooks(author.Id));
        }
    }
}
