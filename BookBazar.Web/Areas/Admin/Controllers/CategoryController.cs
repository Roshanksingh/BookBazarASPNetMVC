using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;
using BookBazar.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBazar.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            //Category? categoryFromDb = _unitOfWork.Categories.Find(id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }



        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            //Category? categoryFromDb = _unitOfWork.Categories.Find(id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            Category? categoryObj = _unitOfWork.Category.Get(c => c.Id == id);

            if (categoryObj == null) { return NotFound(); }

            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
