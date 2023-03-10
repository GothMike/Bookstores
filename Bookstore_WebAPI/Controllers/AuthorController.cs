using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public async Task<IActionResult> GetAuthorsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authorService.GetAllMappingEntitiesAsync());
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAuthorAsync(int authorId)
        {
            if (!await _authorService.EntityExistsAsync(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authorService.GetMappingEntityAsync(authorId));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAuthorAsync([FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authorService.CreateMappingAuthorAsync(authorDto))
            {
                ModelState.AddModelError("", "Что-то пошло не так при создании автора");
                return StatusCode(500, ModelState);
            }

            return Ok("Успешно создано");
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAuthorAsync(int authorId, [FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
                return BadRequest();

            if (authorId != authorDto.Id)
                return BadRequest();

            if (!await _authorService.EntityExistsAsync(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authorService.UpdateMappingEntity(authorDto))
            {
                ModelState.AddModelError("", "Что-то пошло не так при редактировании автора");
                return StatusCode(500, ModelState);
            }

            return Ok($"Автор отредактирован в базе данных");
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAuthorAsync(int authorId)
        {
            if (!await _authorService.EntityExistsAsync(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authorService.DeleteEntityAsync(authorId))
            {
                ModelState.AddModelError("", "Что-то пошло не так при удалении автора");
                return StatusCode(500, ModelState);
            }

            return Ok($"Автор удален из базы данных");
        }
    }
}
