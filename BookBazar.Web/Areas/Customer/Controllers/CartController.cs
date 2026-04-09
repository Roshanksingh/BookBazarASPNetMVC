using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;
using BookBazar.Models.ViewModels;
using BookBazar.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BookBazar.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM CartVM { get; set; } = new();

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();

            CartVM = new ShoppingCartVM
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == userId,
                    includeProperties: "Product"
                )
            };

            foreach (var cart in CartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
                CartVM.TotalQuantity += cart.Count;
            }

            return View(CartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb != null)
            {
                if (cartFromDb.Count <= 1)
                {
                    _unitOfWork.ShoppingCart.Remove(cartFromDb);
                }
                else
                {
                    cartFromDb.Count -= 1;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }

                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb != null)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var userId = GetCurrentUserId();

            CartVM = new ShoppingCartVM
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == userId,
                    includeProperties: "Product"
                ),
                OrderHeader = new OrderHeader()
            };

            CartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            if (CartVM.OrderHeader.ApplicationUser != null)
            {
                CartVM.OrderHeader.Name = CartVM.OrderHeader.ApplicationUser.Name;
                CartVM.OrderHeader.PhoneNumber = CartVM.OrderHeader.ApplicationUser.PhoneNumber;
                CartVM.OrderHeader.StreetAddress = CartVM.OrderHeader.ApplicationUser.StreetAddress;
                CartVM.OrderHeader.City = CartVM.OrderHeader.ApplicationUser.City;
                CartVM.OrderHeader.State = CartVM.OrderHeader.ApplicationUser.State;
                CartVM.OrderHeader.PostalCode = CartVM.OrderHeader.ApplicationUser.PostalCode;
            }

            foreach (var cart in CartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
                CartVM.TotalQuantity += cart.Count;
            }

            return View(CartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var userId = GetCurrentUserId();

            CartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product"
            );

            if (!CartVM.ShoppingCartList.Any())
            {
                TempData["error"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            CartVM.OrderHeader.OrderDate = DateTime.Now;
            CartVM.OrderHeader.ApplicationUserId = userId;
            CartVM.OrderHeader.OrderStatus = SD.StatusPending;
            CartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;

            foreach (var cart in CartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
                CartVM.TotalQuantity += cart.Count;
            }

            _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in CartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = CartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };

                _unitOfWork.OrderDetail.Add(orderDetail);
            }

            _unitOfWork.Save();

            var domain = $"{Request.Scheme}://{Request.Host.Value}/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={CartVM.OrderHeader.Id}",
                CancelUrl = domain + $"Customer/Cart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in CartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };

                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeader.UpdateStripePaymentId(
                CartVM.OrderHeader.Id,
                session.Id,
                session.PaymentIntentId
            );

            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return StatusCode(303);
        }

        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

            if (orderHeader == null)
            {
                return NotFound();
            }

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase))
                {
                    _unitOfWork.OrderHeader.UpdateStatus(
                        orderHeader.Id,
                        SD.StatusApproved,
                        SD.PaymentStatusApproved
                    );

                    var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                        u => u.ApplicationUserId == orderHeader.ApplicationUserId
                    );

                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCartList);
                    _unitOfWork.Save();
                }
            }

            return View(orderHeader.Id);
        }

        private string GetCurrentUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}