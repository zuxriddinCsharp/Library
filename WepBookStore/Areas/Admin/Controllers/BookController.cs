using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WepBookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class BookController : Controller
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager ;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string RootPath;
    public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        RootPath = _webHostEnvironment.WebRootPath;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        List<Book> objBookList = _unitOfWork.Book.GetAll(includeProperties: "Genre").ToList();
        return View(objBookList);
    }
    public IActionResult Upsert(int? id)
    {

        BookVM bookVM = new()
        {
            GenreList = _unitOfWork.Genre
            .GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Book = new Book()
        };
        if (id == null || id == 0)
        {
            return View(bookVM);
        }
        else
        {
            bookVM.Book = _unitOfWork.Book.Get(u => u.Id == id);
            return View(bookVM);
        }

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(BookVM bookVM, IFormFile file,IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image == null)
            {
                return BadRequest("You didn't enter the image");
            }
            if (file is null)
            {
                return BadRequest("You didn't enter the file");
            }
            //Avval mavjud fileni o'chiradi
            //Bu yerda oldin mavjud kodni funksiyaga chiqardim pastda
            DeleteOldFile(bookVM);
            
            //saqlaydi SaveFileToRoot methodi pastda,
            bookVM.Book.ImageUrl = @"\images\book\" + SaveFileToRoot(image, "images");
            bookVM.Book.FilePath = @"\files\book\" + SaveFileToRoot(file, "files");

            if (bookVM.Book.Id == 0)
            {
                var user = await _userManager.GetUserAsync(User);
                bookVM.Book.UserId = (user.Id);
                _unitOfWork.Book.Add(bookVM.Book);
            }
            else
            {
                _unitOfWork.Book.Update(bookVM.Book);
            }


            _unitOfWork.Save();
            TempData["success"] = "Book created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            bookVM.GenreList = _unitOfWork.Genre
            .GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }); 
            return View(bookVM);
        }
    }
    private void DeleteOldFile(BookVM bookVM)
    {
        if (string.IsNullOrEmpty(bookVM.Book.ImageUrl) || string.IsNullOrEmpty(bookVM.Book.FilePath))
        {
            return;
        }
        var oldImagePath =
            Path.Combine(RootPath, bookVM.Book.ImageUrl.TrimStart('\\'));
        
        var oldFilePath =
            Path.Combine(RootPath, bookVM.Book.FilePath.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
            System.IO.File.Delete(oldFilePath);
        }
    }
    private string SaveFileToRoot(IFormFile file,string path)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string bookPath = Path.Combine(RootPath, @$"{path}\book");
        
        using (var fileSteram = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
        {
            file.CopyTo(fileSteram);
        }
        return fileName;
    }

    #region API CLASS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = (await _userManager.GetUserAsync(User)).Id;

        List<Book> objBookList = _unitOfWork.Book.GetAll(includeProperties: "Genre")
            .Where(x => x.UserId == userId)
            .ToList();
        return Json(new {data=objBookList});
    }
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var bookToBeDelete = _unitOfWork.Book.Get(u=>u.Id == id);
        if (bookToBeDelete == null)
        {
            return Json(new {success =false, message="Error while deleting"});
        }
        var oldImagePath =
                       Path.Combine(_webHostEnvironment.WebRootPath, bookToBeDelete.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Book.Remove(bookToBeDelete);
        _unitOfWork.Save();
        TempData["success"] = "Book deleted successfully";
        return Json(new {success=true, message="Delite Successful" });
    }
    #endregion
}
