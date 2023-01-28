using Microsoft.AspNetCore.OutputCaching;
using OutputCaching.Sample.Model;

namespace OutputCaching.Sample.Service
{
    public interface IProductService
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Product GetProductById(int id);
        List<Product> GetProducts();
        void RemoveProduct(Product product);
    }

    public class ProductService : IProductService
    {
        private List<Product> products = new List<Product>
        {
            new Product { Id = 1 , Name = "Book", Price = 20.5m},
            new Product { Id = 2 , Name = "Computer", Price = 999.99m},
            new Product { Id = 3 , Name = "Keyboard", Price = 39.99m},
            new Product { Id = 4 , Name = "Mouse", Price = 19.99m},
        };

        private readonly IOutputCacheStore _cache;

        public ProductService(IOutputCacheStore cache)
        {
            _cache = cache;
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public Product GetProductById(int id)
        {
            return products.Where(c => c.Id == id).FirstOrDefault()!;

        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            products.Remove(product);
        }
    }
}
