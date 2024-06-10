using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repasitory.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.DataAccess.Repository;

public class Repository <T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        this.dbSet=_context.Set<T>();
        _context.Books.Include(u => u.Genre).Include(u => u.GenreId);
    }
    public void Add(T entity)
    {
         dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        query=query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(string? includeProperties=null)
    {
        IQueryable<T> query = dbSet;
        if(!string.IsNullOrEmpty(includeProperties))
        {
            foreach(var property in includeProperties
                .Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}
