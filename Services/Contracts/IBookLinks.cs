using Entities.DTOs;
using Entities.LogModel;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpcontext);
        //LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, Guid authorId, HttpContext httpcontext);
    }
}
