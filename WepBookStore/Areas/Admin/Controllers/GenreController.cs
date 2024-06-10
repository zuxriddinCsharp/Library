using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WepBookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles =SD.Role_Admin)]
public class GenreController : Controller
{
    
    private readonly IUnitOfWork _unitOfWork;
    public GenreController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index(string? searchString = "")
    {
        List<Genre> objGenreList = _unitOfWork.Genre.GetAll().ToList();

        ViewBag.SearchString = searchString ?? "";
        
        if (!string.IsNullOrEmpty(searchString))
        {
            objGenreList = objGenreList.Where(x => x.Name.Contains(searchString)).ToList();
        }
        return View(objGenreList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Genre obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Genre.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Genre created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int? id)
    {
        if (id == 0)
        {
            return NotFound();
        }
        var Genre = _unitOfWork.Genre.Get(c => c.Id == id);
        if (Genre == null)
        {
            return NotFound();
        }
        return View(Genre);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Genre obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Genre.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Genre updated successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == 0)
        {
            return NotFound();
        }
        var Genre = _unitOfWork.Genre.Get(c => c.Id == id);
        if (Genre == null)
        {
            return NotFound();
        }
        return View(Genre);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var Genre = _unitOfWork.Genre.Get(c => c.Id == id);
        if (Genre == null)
        {
            return NotFound();
        }
        _unitOfWork.Genre.Remove(Genre);
        _unitOfWork.Save();
        TempData["success"] = "Genre deleted successfully";
        return RedirectToAction("Index");

    }
}
