using Entities;
using Entities.DTOs;
using Entities.Exceptions;
using Repositories.Contracts;
using Services.Contracts;
using Services.Utilities.Mapperly;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly AppMapperly _mapperly = new();

        public BookManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDto bookDto)
        {
            var entity = _mapperly.ToEntityBookDto(bookDto);
            _repositoryManager.BookRepository.CreateOneBook(entity);
            await _repositoryManager.SaveAsync();

            return _mapperly.BookDtoToBook(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);

            _repositoryManager.BookRepository.DeleteOneBook(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _repositoryManager.BookRepository.GetAllBooksAsync(trackChanges);
            return _mapperly.BookToDtoList(books.ToList());
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);
            return _mapperly.BookDtoToBook(book);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);
            if (book == null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapperly.ToDto(book);
            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapperly.ToEntityTwoEntity(bookDtoForUpdate, book);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);
            entity = _mapperly.ToEntity(bookDtoForUpdate);

            _repositoryManager.BookRepository.Update(entity);
            await _repositoryManager.SaveAsync();
        }

        private async Task<Book> GetOneBookByIdAndCheckExists(int id, bool trackChanges)
        {
            var entity = await _repositoryManager.BookRepository.GetOneBookByIdAsync(id, trackChanges);

            if (entity == null)
                throw new BookNotFoundException(id);

            return entity;
        }
    }
}
