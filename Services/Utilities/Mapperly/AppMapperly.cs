using Entities;
using Entities.DTOs;
using Riok.Mapperly.Abstractions;

namespace Services.Utilities.Mapperly
{
    [Mapper]
    public partial class AppMapperly
    {
        public partial BookDto BookDtoToBook(Book book);
        public partial Book ToEntityBookDto(BookDto bookDto);

        public partial Book ToEntity(BookDtoForUpdate bookDto);
        public partial BookDtoForUpdate ToDto(Book book);


        public partial List<BookDtoForUpdate> ToDtoList(List<Book> books);
        public partial List<BookDto> BookToDtoList(List<Book> books);

        public partial void ToEntityTwoEntity(BookDtoForUpdate source, Book target);


    }
}


//[MapperIgnoreTarget(nameof(Book.Id))]
//public partial void UpdateEntity(BookDtoForUpdate source, Book target);