using Entities.DTOs;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        public ServiceManager(IRepositoryManager repositoryManager, IBookLinks bookLinks)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, bookLinks));
        }

        public IBookService BookService => _bookService.Value;
    }
}
