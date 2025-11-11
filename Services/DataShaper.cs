using Entities.Models;
using Services.Contracts;
using System.Dynamic;
using System.Reflection;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string? fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFields);
        }

        public ShapedEntity ShapeData(T entity, string? fieldsString)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string? fieldsString)
        {
            if (string.IsNullOrWhiteSpace(fieldsString))
                return Properties;

            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(f => f.Trim());

            return Properties
                .Where(pi => fields.Contains(pi.Name, StringComparer.InvariantCultureIgnoreCase));
        }


        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var data = new ShapedEntity();

            foreach (var item in requiredProperties)
            {
                var objectPropertyValue = item.GetValue(entity);
                data.Entity.TryAdd(item.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");
            data.Id= (int)objectProperty.GetValue(entity);

            return data;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            foreach (var item in entities)
                yield return FetchDataForEntity(item, requiredProperties);
        }

        //  yield → “Veriyi nasıl ürettiğinle ilgilidir.”
        //  Pagination → “Veriyi ne kadar çektiğinle ilgilidir.”
        //  Kullanıcının gördüğü şey → “Verinin ne zaman gönderildiğiyle ilgilidir.”

    }
}
