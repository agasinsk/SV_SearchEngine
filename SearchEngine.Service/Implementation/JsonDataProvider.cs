using Newtonsoft.Json;
using SearchEngine.Service.Interface;
using System;

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

        public T GetData()
        {
            var fileContent = _fileContentProvider.GetFileContent();
            var data = JsonConvert.DeserializeObject<T>(fileContent);

            return data;
        }
    }
}