using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SearchEngine.Model.Enum;
using SearchEngine.Service.Interface;
using System;
using System.Threading.Tasks;

namespace SearchEngine.Service.Implementation
{
    public class JsonDataProvider<T> : IDataProvider<T>
        where T : class
    {
        private readonly IFileContentProvider _fileContentProvider;
        private readonly IMemoryCache _cache;

        public JsonDataProvider(IFileContentProvider fileContentProvider, IMemoryCache cache)
        {
            _fileContentProvider = fileContentProvider ?? throw new ArgumentNullException(nameof(fileContentProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<T> GetData()
        {
            if (!_cache.TryGetValue(CacheKey.Data, out T data))
            {
                var fileContent = await _fileContentProvider.GetFileContent();
                data = JsonConvert.DeserializeObject<T>(fileContent);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(CacheKey.Data, data, cacheEntryOptions);
            }

            return data;
        }
    }
}