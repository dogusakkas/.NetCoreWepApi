using Entities.DTOs;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly IDataShaper<BookDto> _dataShaper;
        public ServiceManager(IRepositoryManager repositoryManager, IDataShaper<BookDto> dataShaper)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, dataShaper));
        }

        public IBookService BookService => _bookService.Value;
    }
}
