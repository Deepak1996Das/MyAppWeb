using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;

namespace MyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.category.GetAll();
            return View(categories);
        }


        // Create category----------------------------------------------------


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created Done!";
                return RedirectToAction("Index");
            }

            return View(category);

        }


        //Edit Category-------------------------------------------------------


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _unitOfWork.category.GetById(u => u.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category edited Done!";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //Delete----------------------------------------------------------


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _unitOfWork.category.GetById(u => u.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult PostDelete(int? id)
        {
            if (ModelState.IsValid)
            {
                Category category = _unitOfWork.category.GetById(u => u.id == id);
                _unitOfWork.category.Delete(category);
                _unitOfWork.Save();
                TempData["success"] = "Category deleted Done!";
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
