using AutoMapper;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services.Interfaces;

namespace Bookstore_WebAPI.Data.Services
{
    public class PublishingHouseService : IPublishingHouseService
    {
        private readonly IPublishingHouseRepository _publishingHouseRepository;
        private readonly IMapper _mapper;

        public PublishingHouseService(IPublishingHouseRepository publishingHouseRepository, IMapper mapper)
        {
            _publishingHouseRepository = publishingHouseRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateMappingPublishingHouseAsync(PublishingHouseDto entityDto)
        {
            var publishingHouse = MappingEntity(entityDto);
            var create = await _publishingHouseRepository.CreatePublishingHouseAsync(publishingHouse);
            return create;
        }

        public async Task<bool> DeleteEntityAsync(int id)
        {
            var publishingHouse = await _publishingHouseRepository.GetAsync(id);

            return await _publishingHouseRepository.DeleteAsync(publishingHouse);
        }

        public Task<bool> EntityExistsAsync(int id)
        {
            return _publishingHouseRepository.EntityExistsAsync(id);
        }

        public async Task<ICollection<PublishingHouseDto>> GetAllMappingEntitiesAsync()
        {
            return _mapper.Map<ICollection<PublishingHouseDto>>(await _publishingHouseRepository.GetAllAsync());
        }

        public async Task<PublishingHouseDto> GetMappingEntityAsync(int id)
        {
            return _mapper.Map<PublishingHouseDto>(await _publishingHouseRepository.GetAsync(id));
        }

        public PublishingHouse MappingEntity(PublishingHouseDto entityDto)
        {
            return _mapper.Map<PublishingHouse>(entityDto);
        }

        public Task<bool> UpdateMappingEntity(PublishingHouseDto entity)
        {
            var publishingHouse = MappingEntity(entity);

            return _publishingHouseRepository.UpdateAsync(publishingHouse);
        }
    }
}
