using Newtonsoft.Json;
using SearchEngine.Service.Interface;
using System;
using System.Threading.Tasks;

namespace SearchEngine.Service.Implementation
{
    public class JsonDataProvider<T> : IDataProvider<T>
        where T : class
    {
        private readonly IFileContentProvider _fileContentProvider;

        public JsonDataProvider(IFileContentProvider fileContentProvider)
        {
            _fileContentProvider = fileContentProvider ?? throw new ArgumentNullException(nameof(fileContentProvider));
        }

        public async Task<T> GetData()
        {
            var fileContent = await _fileContentProvider.GetFileContent();
            var data = JsonConvert.DeserializeObject<T>(fileContent);

            return data;
        }
    }
}