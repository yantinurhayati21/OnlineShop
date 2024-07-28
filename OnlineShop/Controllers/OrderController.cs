using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Linq;

namespace OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Order
        [Authorize]
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public async Task<IActionResult> Create(int Id)
        {
            var order = await _context.Orders.Where(o => o.Id == Id).FirstOrDefaultAsync();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] OrderForm data, int Id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(data);
            }

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
                if (product == null)
                {
                    return NotFound("Produk tidak ditemukan");
                }

                // Pastikan jumlah produk yang dipesan tidak melebihi stok yang tersedia
                if (product.Stok < data.qty)
                {
                    ModelState.AddModelError("", "Stok produk tidak mencukupi");
                    ViewBag.Products = await _context.Products.ToListAsync();
                    return View(data);
                }

                var order = new Order
                {
                    CustomerName = data.CustomerName,
                    Address = data.Address,
                    PhoneNumber = data.PhoneNumber,
                    Product = product,
                    qty = data.qty, // Tambahkan quantity ke order
                    Status = "Pesanan Belum Diterima"
                };

                // Kurangi stok produk
                product.Stok -= data.qty;

                _context.Orders.Add(order);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Tangani kesalahan
                ModelState.AddModelError("", "Terjadi kesalahan saat menyimpan pesanan: " + ex.Message);
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(data);
            }
        }

        [Authorize]
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Include(o => o.Product).FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize]
        public IActionResult Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }


            order.Status = "pesanan diterima";
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Search(string searchString)
        {
            var orders = _context.Orders.Include(o => o.Product).ToList();

            // Jika ada pencarian
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter pesanan berdasarkan nama pelanggan atau nomor telepon
                orders = orders.Where(o =>
                    o.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    o.PhoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View("Index", orders);
        }
    }
}