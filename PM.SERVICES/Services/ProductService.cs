using AutoMapper;
using Microsoft.Extensions.Logging;
using PM.CORE;
using PM.CORE.DTOs;
using PM.CORE.Entities;
using PM.CORE.Interfaces;
using PM.INFRASTRUCTURE.Cache;

namespace PM.SERVICES.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        // Events for operation handling
        public event ProductOperationHandler OnProductCreated;
        public event ProductOperationHandler OnProductUpdated;
        public event ProductOperationHandler OnProductDeleted;
        public event ProductOperationErrorHandler OnError;

        public ProductService(
            IProductRepository repository,
            ICacheService cacheService,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product with ID: {Id}", id);
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all products");
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var createdProduct = await _repository.AddAsync(product);

                var createdDto = _mapper.Map<ProductDto>(createdProduct);
                OnProductCreated?.Invoke(createdDto);

                // Invalidate relevant caches
                _cacheService.Remove(Constants.Cache.ProductListKey);
                _cacheService.Remove(Constants.Cache.CategoryStatsKey);

                return createdDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            try
            {
                var existingProduct = await _repository.GetByIdAsync(id);
                var updatedProduct = _mapper.Map<Product>(productDto);
                updatedProduct.ProductId = id;

                var result = await _repository.UpdateAsync(updatedProduct);
                var updatedDto = _mapper.Map<ProductDto>(result);

                OnProductUpdated?.Invoke(updatedDto);

                // Invalidate relevant caches
                _cacheService.Remove(Constants.Cache.ProductListKey);
                _cacheService.Remove(Constants.Cache.CategoryStatsKey);

                return updatedDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID: {Id}", id);
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(id);

                OnProductDeleted?.Invoke(_mapper.Map<ProductDto>(product));

                // Invalidate relevant caches
                _cacheService.Remove(Constants.Cache.ProductListKey);
                _cacheService.Remove(Constants.Cache.CategoryStatsKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {Id}", id);
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync()
        {
            try
            {
                return await _cacheService.GetOrSetAsync(
                    Constants.Cache.CategoryStatsKey,
                    async () => await _repository.GetCategoryStatisticsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category statistics");
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<CategoryStatsDto> GetHighestStockValueCategoryAsync()
        {
            try
            {
                return await _cacheService.GetOrSetAsync(
                    Constants.Cache.HighestStockValueCategoryKey,
                    async () =>
                    {
                        var categoryName = await _repository.GetHighestStockValueCategoryAsync();
                        var stats = await _repository.GetCategoryStatisticsAsync();
                        return stats.FirstOrDefault(s => s.Category == categoryName);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting highest stock value category");
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetCachedProductListAsync()
        {
            try
            {
                return await _cacheService.GetOrSetAsync(
                    Constants.Cache.ProductListKey,
                    async () =>
                    {
                        var products = await _repository.GetAllAsync();
                        return _mapper.Map<IEnumerable<ProductDto>>(products);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cached product list");
                OnError?.Invoke(ex);
                throw;
            }
        }

        public async Task<CategoryStatsDto> GetCachedCategoryStatsAsync(string category)
        {
            try
            {
                var cacheKey = $"{Constants.Cache.CategoryStatsKey}_{category}";
                return await _cacheService.GetOrSetAsync(
                    cacheKey,
                    async () =>
                    {
                        var allStats = await GetCategoryStatisticsAsync();
                        return allStats.FirstOrDefault(s => s.Category == category);
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cached category stats for category: {Category}", category);
                OnError?.Invoke(ex);
                throw;
            }
        }
    }
}
