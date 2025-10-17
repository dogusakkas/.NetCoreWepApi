using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Services.Contracts;
using Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch.Adapters;

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))] // Log Filter
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BooksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _serviceManager.BookService.GetAllBooksAsync(false);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _serviceManager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto book)
        {
            await _serviceManager.BookService.CreateOneBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))] // Validation Filter
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDtoForUpdate bookDtoForUpdate)
        {
            await _serviceManager.BookService.UpdateOneBookAsync(id, bookDtoForUpdate, true);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _serviceManager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();
        }

        [NonAction]
        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatch(int id, bool trackChanges)
        {
            var (bookDtoForUpdate, book) = await _serviceManager.BookService.GetOneBookForPatchAsync(id, false);
            return (bookDtoForUpdate, book);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            var result = await _serviceManager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, (IObjectAdapter)ModelState);

            TryValidateModel(result.bookDtoForUpdate);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent();
        }
    }
}