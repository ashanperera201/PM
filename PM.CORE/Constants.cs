namespace PM.CORE
{
    public static class Constants
    {
        public static class Cache
        {
            public const int DefaultCacheTimeInMinutes = 5;
            public const string ProductListKey = "ProductList";
            public const string CategoryStatsKey = "CategoryStats";
            public const string HighestStockValueCategoryKey = "HighestStockValueCategory";
        }

        public static class Validation
        {
            public const int MaxProductNameLength = 100;
            public const int MaxCategoryNameLength = 50;
            public const decimal MinProductPrice = 0.01M;
            public const int MinStockValue = 0;
        }
    }
}

namespace PM.CORE.Exceptions
{
    public class ProductNotFoundException : System.Exception
    {
        public ProductNotFoundException(int id)
            : base($"Product with ID {id} was not found.")
        {
        }
    }

    public class ProductValidationException : Exception
    {
        public ProductValidationException(string message)
            : base(message)
        {
        }
    }
}