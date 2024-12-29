using Microsoft.AspNetCore.Mvc;
using PM.CORE.Interfaces;

namespace PM.WEB.Controllers
{
    public class ReportController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IProductService productService, ILogger<ReportController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: Report
        public async Task<IActionResult> Index()
        {
            try
            {
                var categoryStats = await _productService.GetCategoryStatisticsAsync();
                var highestValueCategory = await _productService.GetHighestStockValueCategoryAsync();

                ViewBag.HighestValueCategory = highestValueCategory;
                return View(categoryStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reports");
                return View("Error");
            }
        }
    }
}
