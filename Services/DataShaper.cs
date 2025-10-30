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

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFields);
        }

        public ExpandoObject ShapeData(T entity, string? fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string? fieldsString)
        {
            var requiredProperties = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property == null)
                        continue;

                    requiredProperties.Add(property);
                }
            }
            else
            {
                requiredProperties = Properties.ToList();
            }

            return requiredProperties;
        }

        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var data = new ExpandoObject();

            foreach (var item in requiredProperties)
            {
                var objectPropertyValue = item.GetValue(entity);
                data.TryAdd(item.Name, objectPropertyValue);
            }
            return data;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                var shapedObject = FetchDataForEntity(item, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }
    }
}
