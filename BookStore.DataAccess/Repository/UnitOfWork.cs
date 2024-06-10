using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    public IGenreRepository Genre { get; private set; }
    public IBookRepository Book { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Genre = new GenreRepository(_context);
        Book = new BookRepository(_context);
    }
  

    public void Save()
    {
        _context.SaveChanges();
    }
}
