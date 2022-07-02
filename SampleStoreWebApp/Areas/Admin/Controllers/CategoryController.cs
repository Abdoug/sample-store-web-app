using Microsoft.AspNetCore.Mvc;
using SampleStore.DataAccess;
using SampleStore.DataAccess.Repository.IRepository;
using SampleStore.Models;

namespace SampleStoreWebApp.Areas.Admin.Controllers
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
            IEnumerable<Category> CategoriesList = _unitOfWork.Category.GetAll();

            return View(CategoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            // Begin Validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();

                TempData["success"] = "The entry has been created succesfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been created succesfully";

            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            // Begin Validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();

                TempData["success"] = "The entry has been updated succesfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been updated succesfully";

            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var categoryToBeDeleted = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (categoryToBeDeleted == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(categoryToBeDeleted);
            _unitOfWork.Save();

            TempData["success"] = "The entry has been deleted succesfully";

            return RedirectToAction("Index");
        }
    }
}
