using System.Collections.Generic;
using System.Linq;
using thuchanhWeb.Models; // Thay thế bằng namespace thực tế của bạn

namespace thuchanhWeb.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        public MockProductRepository()
        {
            // Tạo một số dữ liệu mẫu
            _products = new List<Product>
                {
                     new Product { Id = 1, Name = "Laptop", Price = 1000,Description = "A high-end laptop", ImageUrl="https://cdn-i.vtcnews.vn/files/f2/2014/07/29/ban-chai-danh-rang-va-nhung-moi-hoa-0.jpg" },
                     new Product { Id = 2, Name = "Desktop", Price = 2000,Description = "A high-end pc",ImageUrl="https://cdn-i.vtcnews.vn/files/f2/2014/07/29/ban-chai-danh-rang-va-nhung-moi-hoa-0.jpg"},
                     new Product { Id = 3, Name = "Laptop", Price = 3000,Description = "A high-end laptop",ImageUrl="https://cdn-i.vtcnews.vn/files/f2/2014/07/29/ban-chai-danh-rang-va-nhung-moi-hoa-0.jpg"},
                     new Product { Id = 4, Name = "Window", Price = 3000,Description = "A high-end laptop",ImageUrl="https://cdn-i.vtcnews.vn/files/f2/2014/07/29/ban-chai-danh-rang-va-nhung-moi-hoa-0.jpg"},
                    // Thêm các sản phẩm khác
                };
        }
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}