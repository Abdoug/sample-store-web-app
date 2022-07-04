using Microsoft.AspNetCore.Mvc;
using SampleStore.DataAccess.Repository.IRepository;
using SampleStore.Models;
using System.Diagnostics;

namespace SampleStoreWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll("Category,CoverType");

            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, "Category,CoverType");
            ShoppingCart shoppingCart = new()
            {
                Product = product,
                Count = 1,
            };

            return View(shoppingCart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}