using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;

namespace BookStore.DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public void Update(Genre Genre)
        {
            _context.Categories.Update(Genre);
        }
    }
}
