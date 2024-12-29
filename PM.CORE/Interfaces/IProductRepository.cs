using PM.CORE.Entities;
using PM.CORE.DTOs;

namespace PM.CORE.Interfaces
{
    public interface IProductRepository
    {
        // Basic CRUD operations
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);

        // Advanced queries for reports
        Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync();
        Task<string> GetHighestStockValueCategoryAsync();

        // Additional helper methods
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    }
}