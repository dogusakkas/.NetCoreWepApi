using Entities.Models;

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
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string? fieldsString);
        /// <summary>
        /// Veri şekillendirme - tekil
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldsString"></param>
        /// <returns></returns>
        ShapedEntity ShapeData(T entity, string? fieldsString);
    }
}
