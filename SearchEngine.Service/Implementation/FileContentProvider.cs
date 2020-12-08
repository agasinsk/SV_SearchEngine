using Microsoft.Extensions.Options;
using SearchEngine.Model.Configuration;
using SearchEngine.Service.Interface;
using System;
using System.IO;

namespace SearchEngine.Service.Implementation
{
    public class FileContentProvider : IFileContentProvider
    {
        private readonly IOptions<DataSourceOptions> _dataSourceConfiguration;

        public FileContentProvider(IOptions<DataSourceOptions> dataSourceConfiguration)
        {
            _dataSourceConfiguration = dataSourceConfiguration ?? throw new ArgumentNullException(nameof(dataSourceConfiguration));
        }

        public string GetFileContent()
        {
            var filePath = _dataSourceConfiguration.Value.FilePath;
            var fileContent = File.ReadAllText(filePath);

            return fileContent;
        }
    }
}