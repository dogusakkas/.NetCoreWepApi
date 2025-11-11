using Entities.DTOs;
using Entities.LogModel;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Microsoft.AspNetCore.Routing;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Entities.LinkModels;

namespace Services
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpcontext)
        {
            var shapedBooks = ShapedData(booksDto, fields);
            if (ShouldGenerateLinks(httpcontext))
            {
                return ReturnLinkedBooks(booksDto, fields, httpcontext, shapedBooks);
            }

            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList = booksDto.ToList();
            for (int i = 0; i < bookDtoList.Count(); i++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[i], fields);
                shapedBooks[i].Add("Links", bookLinks);
            }

            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLinks = true, LinkedEntites = bookCollection };
        }

        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext, LinkCollectionWrapper<Entity> linkCollectionWrapper)
        {
            linkCollectionWrapper.Links.Add(new Link() 
            { 
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                Rel = "self",
                Method = "GET"
            });
            return linkCollectionWrapper;
        }

        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}/{bookDto.Id}",
                    Rel = "self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel = "create",
                    Method = "POST"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}/{bookDto.Id}",
                    Rel = "remove",
                    Method = "DELETE"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}/{bookDto.Id}",
                    Rel = "update",
                    Method = "PUT"
                }
            };

            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse
            {
                ShapedEntities = shapedBooks
            };
        }

        private bool ShouldGenerateLinks(HttpContext httpcontext)
        {
            var mediaType = (MediaTypeHeaderValue)httpcontext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapedData(IEnumerable<BookDto> booksDto, string fields)
        {
            return _dataShaper.ShapeData(booksDto, fields)
                .Select(x => x.Entity).ToList();
        }


    }
}
