using System.Reflection;
using System.Text;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        /// <summary>
        /// Dinamik sıralama sorgusu oluşturur
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderByQueryString"></param>
        /// <returns></returns>
        public static string CreateOrderQuery<T>(string orderByQueryString) // Generic method
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return string.Empty;

            var orderParams = orderByQueryString.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries); // id desc, title

            var propertyDict = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(x=>x.Name.ToLower(), p=>p.Name); // id -> Id, title -> Title, price -> Price -- Books sınıfı için örnek yazılmıştır.

            var orderQueryBuilder = new StringBuilder();

            // orderByQueryString parametresini işle
            // id desc, title -> "Id descending, Title ascending, price descending"
            foreach (var orderParam in orderParams)
            {
                var parts = orderParam.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries); // ["id", "desc"], ["title"]
                if (parts.Length == 0) continue;

                var propertyName = parts[0].ToLower(); // 
                if (!propertyDict.TryGetValue(propertyName, out var actualPropertyName))
                    continue; // Geçersiz property ismi, atla

                var direction = (parts.Length > 1 && parts[1].StartsWith("desc", StringComparison.OrdinalIgnoreCase))
                    ? "descending"
                    : "ascending";

                orderQueryBuilder.Append($"{actualPropertyName} {direction}, "); // Id descending, Title ascending,
            }

            return orderQueryBuilder.ToString().TrimEnd(',', ' '); // Id descending, Title ascending
        }
    }
}
