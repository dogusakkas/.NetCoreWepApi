using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        /// <summary>
        /// Fiyat aralığına göre filtreleme
        /// </summary>
        /// <param name="books"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Arama işlemi - FULLTEXT CONTAINS
        /// </summary>
        /// <param name="books"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Dinamik sıralama
        /// </summary>
        /// <param name="books"></param>
        /// <param name="orderByQueryString"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string? orderByQueryString) // Generic method
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderBy("Id"); // Varsayılan sıralama

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString); // Generic Order Query

            if (string.IsNullOrWhiteSpace(orderQuery))
                return query.OrderBy("Id");

            return query.OrderBy(orderQuery); // System.Linq.Dynamic.Core kullanımı
        }
    }
}