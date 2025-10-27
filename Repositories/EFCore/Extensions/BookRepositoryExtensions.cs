using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooksByPrice(this IQueryable<Book> books, uint? minPrice, uint? maxPrice)
        {
            if (minPrice.HasValue)
            {
                books = books.Where(b => b.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                books = books.Where(b => b.Price <= maxPrice.Value);
            }
            return books;
        }

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return books;
            }

            // FULL-TEXT CONTAINS kullanımı
            var term = $"\"{searchTerm.Trim()}*\"";

            return books.Where(b => EF.Functions.Contains(b.Title, term));



            //return books.FromSqlInterpolated($"SELECT * FROM Books WHERE CONTAINS(Title, 'FORMSOF(INFLECTIONAL, {searchTerm})')"); // INFLECTIONAL ile kök kelime aranır


            //// FULLTEXT CONTAINS kullanımı
            //searchTerm = $"\"{searchTerm.Trim()}*\""; // wildcard ile başlat
            //return books.FromSqlInterpolated($"SELECT * FROM Books WHERE CONTAINS(Title, {searchTerm})");

            // SQL LIKE için % eklenir
            //searchTerm = $"%{searchTerm.Trim()}%"; 

            // SQL LIKE kullanımı
            //return books.Where(x => EF.Functions.Like(x.Title, searchTerm)); 
        }
    }
}
