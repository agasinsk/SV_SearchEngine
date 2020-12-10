#region Usings

using Microsoft.Extensions.DependencyInjection;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Implementation.SearchConfiguration;
using SearchEngine.Service.Interface;

#endregion

namespace SearchEngine.Web.Configuration
{
    public static class ServiceModule
    {
        public static IServiceCollection AddServiceModule(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDataProvider<>), typeof(JsonDataProvider<>));
            services.AddTransient<IFileContentProvider, FileContentProvider>();
            services.AddTransient<ISearchService, ISearchService>();
            services.AddTransient<ISearchEvaluator, SearchEvaluator>();
            services.AddTransient<ISearchConfigurationFactory, SearchConfigurationFactory>();

            return services;
        }
    }
}