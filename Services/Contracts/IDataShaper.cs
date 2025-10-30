using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T>
    {
        /// <summary>
        /// Veri şekillendirme - çoğul
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString);
        /// <summary>
        /// Veri şekillendirme - tekil
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        ExpandoObject ShapeData(T entity, string? fieldsString);
    }
}
