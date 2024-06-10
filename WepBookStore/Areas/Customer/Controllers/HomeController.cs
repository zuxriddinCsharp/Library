using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System.Diagnostics;

namespace WepBookStore.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly FileExtensionContentTypeProvider _fileExtension;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string RootPath;
    public HomeController(ILogger<HomeController> logger,
        IUnitOfWork unitOfWork,
        FileExtensionContentTypeProvider fileExtension,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _fileExtension = fileExtension;
        _webHostEnvironment = webHostEnvironment;
        RootPath = _webHostEnvironment.WebRootPath;
    }

    public IActionResult Index(string? search,string? author,int? genre)
    {
        IEnumerable<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Genre");
        
        var authors = bookList.Select(x => x.Author).ToList();
        var genres = _unitOfWork.Genre.GetAll().ToList();

        ViewBag.Authors = new SelectList(authors,author ?? "All");
        ViewBag.Genres = new SelectList(genres,"Id", "Name",genre ?? 0);
        ViewBag.Search = search ?? string.Empty;

        if(search is not null)
        {
            search = search.ToLower();
        }

        if (!string.IsNullOrEmpty(search))
        {
            bookList = bookList.Where(
                   b => b.Author.ToLower().Contains(search)
                || b.Title.ToLower().Contains(search)
                || b.Genre.Name.ToLower().Contains(search));
        }
        if (!string.IsNullOrEmpty(author))
        {
            bookList = bookList.Where(b => b.Author.Contains(author));
        }
        if(genre.HasValue && genre != 0)
        {
            bookList = bookList.Where(b => b.GenreId == genre);
        }

        return View(bookList.OrderBy(b => b.Title).ToList());
    }
    public IActionResult Details(int bookId)
    {
       Book book = _unitOfWork.Book.Get(u=>u.Id==bookId, includeProperties: "Genre");
        return View(book);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Download(string filePath)
    {
        string downloadPath = Path.Combine(RootPath + filePath);

        if (System.IO.File.Exists(downloadPath))
        {
            if (!_fileExtension.TryGetContentType(downloadPath, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(downloadPath);
            return (File(bytes, contentType));
        }
        return NotFound();
    }
}
