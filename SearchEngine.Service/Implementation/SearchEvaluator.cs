using SearchEngine.Model.Interface;
using SearchEngine.Service.Interface;
using System;

namespace SearchEngine.Service.Implementation
{
    public class SearchEvaluator : ISearchEvaluator
    {
        private readonly ISearchConfigurationFactory _searchConfigurationFactory;

        public SearchEvaluator(ISearchConfigurationFactory searchConfigurationFactory)
        {
            _searchConfigurationFactory = searchConfigurationFactory ?? throw new ArgumentNullException(nameof(searchConfigurationFactory));
        }

        public (int weight, int transitiveWeight) Evaluate(ISearchable searchable, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return (0, 0);
            }

            var searchConfiguration = _searchConfigurationFactory.GetSearchConfiguration(searchable);

            var properties = searchable.GetType().GetProperties();
            int totalWeight = 0, totalTransitiveWeight = 0;
            var searchValue = searchString.ToLower();

            foreach (var property in properties)
            {
                var (weight, transitiveWeight) = searchConfiguration.GetWeightsForProperty(property.Name);

                if (weight <= 0 && transitiveWeight <= 0)
                {
                    continue;
                }

                var propertyValue = property.GetValue(searchable);

                if (propertyValue != null && propertyValue.ToString().ToLower().Contains(searchValue))
                {
                    totalWeight += weight;
                    totalTransitiveWeight += transitiveWeight;
                }
            }

            return (totalWeight, totalTransitiveWeight);
        }
    }
}