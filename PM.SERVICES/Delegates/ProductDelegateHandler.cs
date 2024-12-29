using Microsoft.Extensions.Logging;
using PM.CORE.DTOs;
using PM.CORE.Interfaces;

namespace PM.SERVICES.Delegates
{
    public class ProductDelegateHandler
    {
        private readonly ILogger<ProductDelegateHandler> _logger;

        public ProductDelegateHandler(ILogger<ProductDelegateHandler> logger)
        {
            _logger = logger;
        }

        public void HandleProductCreated(ProductDto product)
        {
            _logger.LogInformation("Product created: {ProductName} in category {Category}",
                product.Name, product.Category);
        }

        public void HandleProductUpdated(ProductDto product)
        {
            _logger.LogInformation("Product updated: {ProductName} in category {Category}",
                product.Name, product.Category);
        }

        public void HandleProductDeleted(ProductDto product)
        {
            _logger.LogInformation("Product deleted: {ProductName} from category {Category}",
                product.Name, product.Category);
        }

        public void HandleError(Exception ex)
        {
            _logger.LogError(ex, "Product operation error occurred");
        }

        public void RegisterHandlers(IProductService productService)
        {
            productService.OnProductCreated += HandleProductCreated;
            productService.OnProductUpdated += HandleProductUpdated;
            productService.OnProductDeleted += HandleProductDeleted;
            productService.OnError += HandleError;
        }

        public void UnregisterHandlers(IProductService productService)
        {
            productService.OnProductCreated -= HandleProductCreated;
            productService.OnProductUpdated -= HandleProductUpdated;
            productService.OnProductDeleted -= HandleProductDeleted;
            productService.OnError -= HandleError;
        }
    }
}
