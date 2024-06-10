using BookStore.DataAccess.Repasitory.IRepository;
using BookStore.Models;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book book);
    }
}
