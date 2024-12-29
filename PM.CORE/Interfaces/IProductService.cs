using PM.CORE.DTOs;

namespace PM.CORE.Interfaces
{
    // Delegate definitions
    public delegate void ProductOperationHandler(ProductDto product);
    public delegate void ProductOperationErrorHandler(Exception ex);

    public interface IProductService
    {
        // Event handlers for operations
        event ProductOperationHandler OnProductCreated;
        event ProductOperationHandler OnProductUpdated;
        event ProductOperationHandler OnProductDeleted;
        event ProductOperationErrorHandler OnError;

        // Basic CRUD operations
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto);
        Task DeleteProductAsync(int id);

        // Report-related operations
        Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync();
        Task<CategoryStatsDto> GetHighestStockValueCategoryAsync();

        // Cache-related operations (these will be implemented with different caching strategies)
        Task<IEnumerable<ProductDto>> GetCachedProductListAsync();
        Task<CategoryStatsDto> GetCachedCategoryStatsAsync(string category);
    }
}