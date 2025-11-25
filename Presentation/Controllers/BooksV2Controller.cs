using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{

    [ApiController]
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/books")]
    public class BooksV2Controller : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BooksV2Controller(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _serviceManager.BookService.GetAllBooksAsyncV2(false);
            var booksV2 = books.Select(x=> new
            {
                Title = x.Title,
                Id = x.Id
            });
            return Ok(books);
        }
    }
}
