using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using thuchanhWeb.Models; // Thay thế bằng namespace thực tế của bạn
using thuchanhWeb.Repositories; // Thay thế bằng namespace thực tế của bạn


namespace thuchanhWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // 🟢 Kiểm tra và lưu ảnh đại diện
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                // 🟢 Kiểm tra và lưu danh sách ảnh
                if (imageUrls != null && imageUrls.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }

            // 🟠 Load lại danh sách danh mục nếu có lỗi nhập liệu
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View(product);
        }

        // Các actions khác như Display, Update, Delete
        // Display a list of products
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }// Display a single product
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Show the product update form
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện mới
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                if (imageUrls != null && imageUrls.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        // Lưu các hình ảnh khác mới
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        // Show the product delete confirmation
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Process the product deletion
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]

        private async Task<string> SaveImage(IFormFile image)
        {
            // Thay đổi đường dẫn theo cấu hình của bạn
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}