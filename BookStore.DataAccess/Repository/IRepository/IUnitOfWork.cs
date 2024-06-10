namespace BookStore.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IBookRepository Book { get; }
    IGenreRepository Genre { get; }
    void Save();
}
