using Moq;
using PM.CORE.Interfaces;
using PM.SERVICES.Services;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PM.INFRASTRUCTURE.Cache;
using PM.CORE.Entities;

namespace PM.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ICacheService> _mockCache;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockCache = new Mock<ICacheService>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mockMapper = new Mock<IMapper>();
            _productService = new ProductService(_mockRepository.Object, _mockCache.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnListOfProducts()
        {
            var expectedProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Test Product 1", Category = "Electronics", Price = 100, Stock = 10 },
                new Product { ProductId = 2, Name = "Test Product 2", Category = "Books", Price = 50, Stock = 20 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .Returns(Task.FromResult(expectedProducts.AsEnumerable()));

            var result = await _productService.GetAllProductsAsync();

            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            int productId = 1;
            var expectedProduct = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Category = "Electronics",
                Price = 100,
                Stock = 10
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            var result = await _productService.GetProductByIdAsync(productId);
            
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.ProductId, result.ProductId);
            Assert.Equal(expectedProduct.Name, result.Name);
        }

        [Fact]
        public async Task GetCategoryStatistics_ShouldReturnCorrectStatistics()
        {
            var products = new List<Product>
            {
                new() { Category = "Electronics", Price = 100, Stock = 10 },
                new() { Category = "Electronics", Price = 200, Stock = 5 },
                new() { Category = "Books", Price = 50, Stock = 20 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            var result = await _productService.GetCategoryStatisticsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var electronics = result.First(c => c.Category == "Electronics");
            Assert.Equal(150, electronics.AveragePrice);
            Assert.Equal(15, electronics.TotalStock);
        }
    }
}