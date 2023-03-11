using Bookstore_WebAPI.Data.Models.Dto;
using Bookstore_WebAPI.Data.Models;
using Bookstore_WebAPI.Data.Repository;
using Bookstore_WebAPI.Data.Repository.Interfaces;
using Bookstore_WebAPI.Data.Services;
using Bookstore_WebAPI.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> GetBooksAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _bookService.GetAllMappingEntitiesAsync());
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBookAsync(int bookId)
        {
            if (!await _bookService.EntityExistsAsync(bookId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _bookService.GetMappingEntityAsync(bookId));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookDto bookDto, int authorId, int publishingHouseId)
        {
            if (bookDto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (authorId == null)
                return BadRequest(ModelState);

            if (!await _bookService.CreateMappingBookAsync(bookDto, authorId, publishingHouseId))
            {
                ModelState.AddModelError("", "Что-то пошло не так при создании книги");
                return StatusCode(500, ModelState);
            }

            return Ok("Успешно создано");
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBookAsync(int bookId, [FromBody] BookDto bookDto)
        {
            if (bookDto == null)
                return BadRequest();

            if (bookId != bookDto.Id)
                return BadRequest();

            if (!await _bookService.EntityExistsAsync(bookId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _bookService.UpdateMappingEntity(bookDto))
            {
                ModelState.AddModelError("", "Что-то пошло не так при редактировании книги");
                return StatusCode(500, ModelState);
            }

            return Ok($"Книга отредактирована в базе данных");
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBookAsync(int bookId)
        {
            if (!await _bookService.EntityExistsAsync(bookId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _bookService.DeleteEntityAsync(bookId))
            {
                ModelState.AddModelError("", "Что-то пошло не так при удалении книги");
                return StatusCode(500, ModelState);
            }

            return Ok($"Книга удалена из базы данных");
        }
    }
}
