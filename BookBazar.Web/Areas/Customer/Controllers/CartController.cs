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

        // CartVM is now a plain property — used only to BUILD and PASS data to views.
        // It is NOT marked [BindProperty] so ASP.NET will never fill it from a POST request.
        public ShoppingCartVM CartVM { get; set; } = new();

        // OrderInput IS marked [BindProperty] — this is the ONLY thing ASP.NET will
        // read from the incoming POST body. It only has 6 safe address fields.
        // A malicious user can try to POST OrderTotal=0.01 or PaymentStatus=Approved
        // but the binder will simply ignore them because those fields don't exist here.
        [BindProperty]
        public OrderHeaderInputVM OrderInput { get; set; } = new();

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET — unchanged, builds CartVM from DB and passes it to the view for display
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

        // FIXED: ownership check added — cartFromDb.ApplicationUserId must match the
        // logged-in user. Without this, any authenticated user could POST /Cart/Plus?cartId=5
        // and modify someone else's cart item by guessing the integer ID.
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb == null || cartFromDb.ApplicationUserId != GetCurrentUserId())
                return Forbid();

            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // FIXED: same ownership check
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb == null || cartFromDb.ApplicationUserId != GetCurrentUserId())
                return Forbid();

            if (cartFromDb.Count <= 1)
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if (cartFromDb == null || cartFromDb.ApplicationUserId != GetCurrentUserId())
                return Forbid();

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET Summary — unchanged. Reads user's profile from DB to pre-fill the form.
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

        // POST Summary — the key fix is here.
        // OrderInput was filled by [BindProperty] from the POST body (only 6 safe fields).
        // We never touch CartVM.OrderHeader for sensitive data — we build a fresh OrderHeader
        // entirely on the server and only copy the 6 address fields from OrderInput.
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost()
        {
            var userId = GetCurrentUserId();

            // Always re-fetch cart from the DATABASE — never trust cart data from the client.
            var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product"
            ).ToList();

            if (!shoppingCartList.Any())
            {
                TempData["error"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            // Build a brand-new OrderHeader here on the server.
            // The left side (Name, PhoneNumber etc.) comes from OrderInput — the safe DTO
            //   the customer filled in on the form.
            // The right side (ApplicationUserId, OrderStatus etc.) is set entirely by us —
            //   the server — and can never be overridden by a crafted POST body.
            var orderHeader = new OrderHeader
            {
                // FROM the customer's form (safe — only address info)
                Name = OrderInput.Name,
                PhoneNumber = OrderInput.PhoneNumber,
                StreetAddress = OrderInput.StreetAddress,
                City = OrderInput.City,
                State = OrderInput.State,
                PostalCode = OrderInput.PostalCode,

                // SET BY SERVER — never from the client
                ApplicationUserId = userId,                      // from auth token, not form
                OrderDate = DateTime.Now,                // server clock
                OrderStatus = SD.StatusPending,            // hardcoded
                PaymentStatus = SD.PaymentStatusPending,     // hardcoded
                OrderTotal = 0                            // calculated below from DB prices
            };

            // Calculate OrderTotal from DB product prices — never from what the client sent.
            foreach (var cart in shoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);      // price from DB
                orderHeader.OrderTotal += cart.Price * cart.Count;
            }

            _unitOfWork.OrderHeader.Add(orderHeader);
            _unitOfWork.Save();

            // Create order details using the server-side orderHeader we just saved
            foreach (var cart in shoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = orderHeader.Id,   // using our server-built object
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
            }

            _unitOfWork.Save();

            // Build Stripe session using our server-side orderHeader
            var domain = $"{Request.Scheme}://{Request.Host.Value}/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={orderHeader.Id}",
                CancelUrl = domain + $"Customer/Cart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in shoppingCartList)
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
                orderHeader.Id,
                session.Id,
                session.PaymentIntentId
            );

            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return StatusCode(303);
        }

        // FIXED: ownership check — only the user who placed the order can view its confirmation.
        // Without this, any logged-in user could hit /Cart/OrderConfirmation?id=1,2,3...
        // and see (and trigger cart-clearing for) other users' orders.
        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

            if (orderHeader == null)
                return NotFound();

            // Ownership check — must be your order
            if (orderHeader.ApplicationUserId != GetCurrentUserId())
                return Forbid();

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
                return shoppingCart.Product.Price;
            else if (shoppingCart.Count <= 100)
                return shoppingCart.Product.Price50;
            else
                return shoppingCart.Product.Price100;
        }
    }
}