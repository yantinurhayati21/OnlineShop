using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Data;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(
          AppDbContext context,
          IWebHostEnvironment e
          )
        {
            _context = context;
            _env = e;

        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync(); // Ambil semua produk dari basis data
            return View(products); // Meneruskan daftar produk ke tampilan
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] ProductForm data,
            IFormFile Image
            )
        {

            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var product = new Product()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                Stok = data.Stok,
            };

            if (Image != null)
            {
                var fileFolder = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }

                var fullFile = Path.Combine(fileFolder, Image.FileName);

                using (var stream = System.IO.File.Create(fullFile))
                {
                    await Image.CopyToAsync(stream);
                }

                product.Image = Image.FileName;
            }


            _context.Add(product);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ListProducts()
        {
            var products = await _context.Products.ToListAsync(); // Mengambil daftar akun dari database
            return View(products);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductForm data, IFormFile Image)
        {
            if (id != data.Id || !ModelState.IsValid)
            {
                return View(data);
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = data.Name;
            product.Description = data.Description;
            product.Price = data.Price;
            product.Stok = data.Stok;

            if (Image != null)
            {
                var fileFolder = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }

                var fullFile = Path.Combine(fileFolder, Image.FileName);

                using (var stream = System.IO.File.Create(fullFile))
                {
                    await Image.CopyToAsync(stream);
                }

                product.Image = Image.FileName;
            }
            else // Jika tidak ada gambar yang diunggah, gunakan gambar yang sudah ada
            {
                var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct != null)
                {
                    product.Image = existingProduct.Image;
                }
            }

            _context.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("ListProducts", "Product");
        }




    }
}