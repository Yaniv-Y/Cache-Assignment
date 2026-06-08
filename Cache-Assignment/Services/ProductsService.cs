using Cache_Assignment.Models;
using Microsoft.Extensions.Caching.Memory;
namespace Cache_Assignment.Services
{
    public class ProductsService
    {
        private long _idGen;
        private readonly Dictionary<long, Product> _productsDictionary = [];
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(3));
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IMemoryCache memoryCache, ILogger<ProductsService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task AddProduct(Product product)
        {
            long id = Interlocked.Increment(ref _idGen);
            product.Id = id;
            lock (_productsDictionary)
                _productsDictionary.Add(id, product);
            lock (_memoryCache)
                ((MemoryCache)_memoryCache).Compact(1);
        }

        /// <exception cref="KeyNotFoundException"/>
        public async Task<Product> GetProduct(long id)
        {
            bool hit = true;
            Product? product = await _memoryCache.GetOrCreateAsync(id, async _ =>
            {
                hit = false;
                lock (_productsDictionary)
                    return _productsDictionary[id];
            }, _options);
            _logger.LogInformation("Cache hit: {0}", hit);
            return product!;
        }

        /// <exception cref="KeyNotFoundException"/>
        public async Task UpdateProduct(Product product)
        {
            long id = product.Id;
            lock (_productsDictionary)
            {
                if (!_productsDictionary.ContainsKey(id))
                    throw new KeyNotFoundException();
                _productsDictionary[id] = product;
            }
            lock (_memoryCache)
                if (_memoryCache.TryGetValue(id, out _))
                    _memoryCache.Set(id, product, _options);
        }
    }
}
