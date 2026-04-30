using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionHub.Data;
using FashionHub.Models;

namespace FashionHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _context.Products
                .Where(p => p.IsFeatured)
                .Take(8)
                .ToListAsync();
            return View(featuredProducts);
        }

        public async Task<IActionResult> Search(string q, string category, string sort, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q) || p.Brand.Contains(q) || p.Description.Contains(q));

            if (!string.IsNullOrEmpty(category) && category != "All")
                query = query.Where(p => p.Category == category);

            if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "rating" => query.OrderByDescending(p => p.Rating),
                "newest" => query.OrderByDescending(p => p.Id),
                _ => query.OrderByDescending(p => p.Rating)
            };

            ViewBag.Query = q;
            ViewBag.Category = category;
            ViewBag.Sort = sort;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(await query.ToListAsync());
        }
    }

    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            ViewBag.Category = category;
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var related = await _context.Products
                .Where(p => p.Category == product.Category && p.Id != id)
                .Take(4).ToListAsync();

            ViewBag.RelatedProducts = related;
            return View(product);
        }
    }

    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private string GetSessionId()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CartSessionId")))
                HttpContext.Session.SetString("CartSessionId", Guid.NewGuid().ToString());
            return HttpContext.Session.GetString("CartSessionId")!;
        }

        public async Task<IActionResult> Index()
        {
            var sessionId = GetSessionId();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string size, string color, int quantity = 1)
        {
            var sessionId = GetSessionId();
            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId && c.SelectedSize == size);

            if (existing != null)
                existing.Quantity += quantity;
            else
                _context.CartItems.Add(new CartItem
                {
                    SessionId = sessionId,
                    ProductId = productId,
                    Quantity = quantity,
                    SelectedSize = size,
                    SelectedColor = color
                });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Item added to cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var sessionId = GetSessionId();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();

            if (!cartItems.Any()) return RedirectToAction("Index");
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string customerName, string email, string phone, string address, string city, string pincode)
        {
            var sessionId = GetSessionId();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .ToListAsync();

            if (!cartItems.Any()) return RedirectToAction("Index");

            var order = new Order
            {
                CustomerName = customerName,
                Email = email,
                Phone = phone,
                Address = address,
                City = city,
                Pincode = pincode,
                TotalAmount = cartItems.Sum(c => c.Product!.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Product!.Price,
                    Size = c.SelectedSize,
                    Color = c.SelectedColor
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderSuccess", new { id = order.Id });
        }

        public async Task<IActionResult> OrderSuccess(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            return View(order);
        }
    }
}
