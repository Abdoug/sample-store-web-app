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
            return View();
        }

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
                ProductVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            }

            return View(ProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile? Image)
        {
            bool ProductIsNew = ProductVM.Product.Id == 0;

            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string uploadPath = Path.Combine(wwwRootPath, @"images\products");
                    string extension = Path.GetExtension(Image.FileName);

                    if (ProductVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (FileStream fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
                    {
                        Image.CopyTo(fileStream);
                    }

                    ProductVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if (ProductIsNew)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                } 
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }

                _unitOfWork.Save();

                TempData["success"] = "The entry has been " + (ProductIsNew ? "created" : "updated") + " successfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been " + (ProductIsNew ? "created" : "updated") + " successfully";

            return View(ProductVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll("Category,CoverType");

            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                var getProduct = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string? imagePath = getProduct.ImageUrl;

                if (imagePath != null)
                {
                    var ImageFullPath = Path.Combine(wwwRootPath, imagePath.TrimStart('\\'));

                    if (System.IO.File.Exists(ImageFullPath))
                    {
                        System.IO.File.Delete(ImageFullPath);
                    }
                }

                _unitOfWork.Product.Remove(getProduct);
                _unitOfWork.Save();

                return Json(new { message = "The product has been deleted successfully", status = true });
            }
            catch (Exception e)
            {
                return Json(new { message = "The product has not been deleted successfully", status = false });
            }
        }
        #endregion
    }
}
