using Microsoft.AspNetCore.Mvc;
using SampleStoreWebApp.Data;
using SampleStoreWebApp.Models;

namespace SampleStoreWebApp.Controllers
{
    public class ProductController :Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductsList = _db.Products;

            return View(ProductsList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            // Begin Validation
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();

                TempData["success"] = "The entry has been created succesfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been created succesfully";

            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var productFromDb = _db.Products.Find(id);

            return View(productFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            // Begin Validation
            if (ModelState.IsValid)
            {
                _db.Products.Update(product);
                _db.SaveChanges();

                TempData["success"] = "The entry has been updated succesfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "The entry has not been updated succesfully";

            return View(product);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var productFromDb = _db.Products.Find(id);

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var productToBeDeleted = _db.Products.Find(id);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            _db.Products.Remove(productToBeDeleted);
            _db.SaveChanges();

            TempData["success"] = "The entry has been deleted succesfully";

            return RedirectToAction("Index");
        }
    }
}
