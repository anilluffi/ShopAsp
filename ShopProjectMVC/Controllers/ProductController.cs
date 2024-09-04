using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Models;
using System.Diagnostics;

namespace ShopProjectMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }



        public IActionResult Products()
        {
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Login", "User");
            }
            IEnumerable<Product> products = _productService.GetAll();
            return View(products);
        }



        public IActionResult Create()
        {
            ViewBag.Categories = _productService.GetAllCategories().ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, int category, IFormFile file)
        {
            string hash = Guid.NewGuid().ToString();
            string name = Path.GetFileNameWithoutExtension(file.FileName) + hash + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "pictures", name);
            using (var fileStream = new MemoryStream())
            {
                file.CopyTo(fileStream);
                await System.IO.File.WriteAllBytesAsync(path, fileStream.ToArray());
            }

            product.Image = name;
            product.Category = _productService.GetAllCategories().First(x => x.Id == category);
            await _productService.AddProduct(product);
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            string path = Path.Combine(_env.WebRootPath, "pictures", product.Image);
            await _productService.DeleteProduct(id);
            System.IO.File.Delete(path);
            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            int userId = HttpContext.Session.GetInt32("id").Value;
            await _productService.BuyProduct(userId, id);
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product, int id)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }

            if (id <= 0)
            {
                return BadRequest("Invalid product ID");
            }

            try
            {
                var productFromDb = await _productService.GetProductById(id);
                if (productFromDb == null)
                {
                    return NotFound("Product not found");
                }


                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    ModelState.AddModelError("Name", "Name is required");
                }

                if (product.Price <= 0)
                {
                    ModelState.AddModelError("Price", "Price must be greater than zero");
                }

                if (product.Count < 0)
                {
                    ModelState.AddModelError("Count", "Count cannot be negative");
                }

                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productFromDb.Name = product.Name;
                productFromDb.Description = product.Description;
                productFromDb.Price = product.Price;
                productFromDb.Count = product.Count;

                await _productService.UpdateProduct(productFromDb);
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {

                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }


        }
    }
}
