using Microsoft.EntityFrameworkCore;
using PM.CORE.Entities;
using PM.CORE.Interfaces;
using PM.CORE.DTOs;
using PM.CORE.Exceptions;
using PM.INFRASTRUCTURE.Data;

namespace PM.INFRASTRUCTURE.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await GetByIdAsync(product.ProductId);

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync()
        {
            return await _context.Products
                .GroupBy(p => p.Category)
                .Select(g => new CategoryStatsDto
                {
                    Category = g.Key,
                    AveragePrice = g.Average(p => p.Price),
                    TotalStock = g.Sum(p => p.Stock),
                    StockValue = g.Sum(p => p.Price * p.Stock)
                })
                .ToListAsync();
        }

        public async Task<string> GetHighestStockValueCategoryAsync()
        {
            var result = await _context.Products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    StockValue = g.Sum(p => p.Price * p.Stock)
                })
                .OrderByDescending(x => x.StockValue)
                .FirstOrDefaultAsync();

            return result?.Category;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}