using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleStore.DataAccess;
using SampleStore.DataAccess.Repository.IRepository;
using SampleStore.Models;
using SampleStore.Models.ViewModals;

namespace SampleStoreWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductsList = _unitOfWork.Product.GetAll();

            return View(ProductsList);
        }

        // Get
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                        x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }
                    ),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                        x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }
                    )
            };

            // Create
            if (id == null || id == 0)
            {
            }
            // Update
            else
            {
            }

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;

            return View(ProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile? Image)
        {
            // Begin Validation
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string uploadPath = Path.Combine(wwwRootPath, @"images\products");
                    string extension = Path.GetExtension(Image.FileName);

                    FileStream fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create);

                    Image.CopyTo(fileStream);
                    ProductVM.Product.ImageUrl = @"images\products" + fileName + extension;
                }

                _unitOfWork.Product.Add(ProductVM.Product);
                _unitOfWork.Save();

                TempData["success"] = "The entry has been created succesfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been created succesfully";

            return View(ProductVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            TempData["success"] = "The entry has been deleted succesfully";

            return RedirectToAction("Index");
        }
    }
}
