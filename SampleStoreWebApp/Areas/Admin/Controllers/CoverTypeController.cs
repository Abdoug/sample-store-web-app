using Microsoft.AspNetCore.Mvc;
using SampleStore.DataAccess.Repository.IRepository;
using SampleStore.Models;

namespace SampleStoreWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverType.GetAll();

            return View(coverTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();

                TempData["success"] = "The entry has been created successfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been created";

            return View(coverType);
        }

        public IActionResult Edit(int? id)
        {
            var getEntry = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);

            if (getEntry == null)
            {
                return NotFound();
            }

            return View(getEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();

                TempData["success"] = "The entry has been updated successfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been updated";

            return View(coverType);
        }

        public IActionResult Delete(int? id)
        {
            var getEntry = _unitOfWork.CoverType.GetFirstOrDefault(CoverType => CoverType.Id == id);

            if (getEntry == null)
            {
                return NotFound();
            }


            return View(getEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var getEntry = _unitOfWork.CoverType.GetFirstOrDefault(CoverType => CoverType.Id == id);

            if (getEntry == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(getEntry);
            _unitOfWork.Save();

            TempData["success"] = "The entry has been deleted";

            return RedirectToAction("Index");
        }
    }
}
