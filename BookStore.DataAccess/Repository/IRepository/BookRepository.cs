using BookStore.DataAccess.Data;
using BookStore.Models;

namespace BookStore.DataAccess.Repository.IRepository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Book book)
        {
            var objFromDb = _context.Books.FirstOrDefault(u => u.Id == book.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = book.Title;
                objFromDb.ISBN = book.ISBN;
                objFromDb.Description = book.Description;
                objFromDb.Genre = book.Genre;
                objFromDb.Author = book.Author;

                if(book.ImageUrl != null)
                {
                    objFromDb.ImageUrl = book.ImageUrl;
                }
            }
        }
    }
}
