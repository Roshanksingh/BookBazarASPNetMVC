using BookBazar.DataAccess.Data;
using BookBazar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBazar.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;

        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
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
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            //Category? categoryFromDb = _db.Categories.Find(id);
            Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);

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
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            //Category? categoryFromDb = _db.Categories.Find(id);
            Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);

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
            Category? categoryObj = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (categoryObj == null) { return NotFound(); }

            _db.Categories.Remove(categoryObj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
