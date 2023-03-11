using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bookstore_WebAPI.Data.Services;

namespace Bookstore_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishingHouseController : ControllerBase
    {
        private readonly IPublishingHouseService _publishingHouseService;

        public PublishingHouseController(IPublishingHouseService publishingHouseService)
        {
            _publishingHouseService = publishingHouseService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PublishingHouse>))]
        public async Task<IActionResult> GetPublishingHousesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _publishingHouseService.GetAllMappingEntitiesAsync());
        }

        [HttpGet("{publishingHouseId}")]
        [ProducesResponseType(200, Type = typeof(PublishingHouse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPublishingHouseAsync(int publishingHouseId)
        {
            if (!await _publishingHouseService.EntityExistsAsync(publishingHouseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _publishingHouseService.GetMappingEntityAsync(publishingHouseId));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePublishingHouseAsync([FromBody] PublishingHouseDto publishingHouseDto)
        {
            if (publishingHouseDto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _publishingHouseService.CreateMappingPublishingHouseAsync(publishingHouseDto))
            {
                ModelState.AddModelError("", "Что-то пошло не так при создании");
                return StatusCode(500, ModelState);
            }

            return Ok("Успешно создано");
        }

        [HttpPut("{publishingHouseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePublishingHouseAsync(int publishingHouseId, [FromBody] PublishingHouseDto publishingHouseDto)
        {
            if (publishingHouseDto == null)
                return BadRequest();

            if (publishingHouseId != publishingHouseDto.Id)
                return BadRequest();

            if (!await _publishingHouseService.EntityExistsAsync(publishingHouseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _publishingHouseService.UpdateMappingEntity(publishingHouseDto))
            {
                ModelState.AddModelError("", "Что-то пошло не так при редактировании");
                return StatusCode(500, ModelState);
            }

            return Ok($"Успешно отредактировано");
        }

        [HttpDelete("{publishingHouseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePublishingHouseAsync(int publishingHouseId)
        {
            if (!await _publishingHouseService.EntityExistsAsync(publishingHouseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _publishingHouseService.DeleteEntityAsync(publishingHouseId))
            {
                ModelState.AddModelError("", "Что-то пошло не так при удалении");
                return StatusCode(500, ModelState);
            }

            return Ok($"Успешно удалено");
        }
    }
}
