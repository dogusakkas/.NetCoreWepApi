using Entities;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Runtime.ConstrainedExecution;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneBook(Book book)
        {
            Create(book);
        }

        public void DeleteOneBook(Book book)
        {
            Delete(book);
        }

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            var books = FindAll(trackChanges)
                .OrderBy(x => x.Id);

            return await PagedList<Book>.ToPagedListAsync(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        //EF Core, her LINQ sorgusunu her çağrıda yeniden derler:
        //LINQ ifadeni alır - SQL sorgusuna çevirir - Cache’e alır

        //EF.CompileQuery, sorgu derlenir ve bellekte tutulur, sonraki çağrılarda direkt kullanılır.
        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            //return await GetByIdCompiled(id)!; // 
            return await FindByCondition(x => x.Id == id, trackChanges).FirstOrDefaultAsync();
        }

        public void UpdateOneBook(Book book)
        {
            Update(book);
        }
    }
}
