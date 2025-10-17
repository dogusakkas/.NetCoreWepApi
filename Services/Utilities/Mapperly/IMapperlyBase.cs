using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utilities.Mapperly
{
    public interface IMapperlyBase<TEntity, TDto>
    {
        TDto ToDto(TEntity entity);
        TEntity ToEntity(TDto dto);
        List<TDto> ToDtoList(List<TEntity> entities);
        void MapToEntity(TDto source, TEntity target);
    }

}
