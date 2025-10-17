using Entities.DTOs;
using Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utilities.Mapperly
{
    [Mapper]
    public partial class BookMapperly : IMapperlyBase<Book, BookDto>
    {
        public partial BookDto ToDto(Book entity);
        public partial Book ToEntity(BookDto dto);
        public partial List<BookDto> ToDtoList(List<Book> entities);
        public partial void MapToEntity(BookDto source, Book target);
    }
}
