using AutoMapper;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;

namespace Bookstore_WebAPI.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>();
            CreateMap<PublishingHouse, PublishingHouseDto>();

            CreateMap<AuthorDto, Author>();
            CreateMap<BookDto, Book>();
            CreateMap<PublishingHouse, PublishingHouseDto>();
        }
    }
}
