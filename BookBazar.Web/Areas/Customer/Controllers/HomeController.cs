using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookBazar.Web.Areas.Customer.Controllers
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
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
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

        public IActionResult Details(int productId)
        {
            Product? product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category");

            if (product == null)
            {
                return NotFound();
            }

            ShoppingCart cart = new()
            {
                Product = product,
                Count = 1,
                ProductId = productId
            };

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            cart.ApplicationUserId = userId;

            ShoppingCart? cartFromDB = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId && u.ProductId == cart.ProductId
            );

            if (cartFromDB != null)
            {
                cartFromDB.Count += cart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
