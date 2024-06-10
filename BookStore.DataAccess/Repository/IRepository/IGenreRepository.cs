using BookStore.DataAccess.Repasitory.IRepository;
using BookStore.Models;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IGenreRepository : IRepository<Genre>
    {
        void Update(Genre Genre);
    }
}
