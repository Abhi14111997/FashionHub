using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionHub.Data;
using FashionHub.Models;

namespace FashionHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);
            ViewBag.OutOfStock = await _context.Products.CountAsync(p => p.Stock == 0);
            ViewBag.RecentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();
            return View();
        }

        // ===== PRODUCTS =====
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // Add Product - GET
        public IActionResult AddProduct()
        {
            return View(new Product());
        }

        // Add Product - POST
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "✅ Product successfully added!";
            return RedirectToAction("Products");
        }

        // Edit Product - GET
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // Edit Product - POST
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "✅ Product updated successfully!";
            return RedirectToAction("Products");
        }

        // Delete Product
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "🗑️ Product deleted!";
            }
            return RedirectToAction("Products");
        }

        // Toggle Sold Out
        [HttpPost]
        public async Task<IActionResult> ToggleSoldOut(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Stock = product.Stock > 0 ? 0 : 100;
                await _context.SaveChangesAsync();
                TempData["Success"] = product.Stock == 0 ? "❌ Marked as Sold Out!" : "✅ Marked as In Stock!";
            }
            return RedirectToAction("Products");
        }

        // ===== ORDERS =====
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"✅ Order status updated to {status}!";
            }
            return RedirectToAction("Orders");
        }
    }
}
