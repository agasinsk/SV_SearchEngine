using SearchEngine.Model.Interface;
using SearchEngine.Service.Interface;
using System;

namespace SearchEngine.Service.Implementation
{
    public class SearchEvaluator : ISearchEvaluator
    {
        private const int FullMatchMultiplier = 10;
        private const int PartialMatchMultiplier = 1;
        private const int NoMatchMultiplier = 0;

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
            var searchValue = searchString.Trim().ToLower();

            foreach (var property in properties)
            {
                var (weight, transitiveWeight) = searchConfiguration.GetWeightsForProperty(property.Name);

                if (weight <= 0 && transitiveWeight <= 0)
                {
                    continue;
                }

                var propertyValue = property.GetValue(searchable);
                var matchMultplier = GetMatchMultiplier(propertyValue, searchValue);

                totalWeight += matchMultplier * weight;
                totalTransitiveWeight += matchMultplier * transitiveWeight;
            }

            return (totalWeight, totalTransitiveWeight);
        }

        private int GetMatchMultiplier(object propertyValue, string searchQuery)
        {
            var propertyText = propertyValue?.ToString().Trim().ToLower();

            if (string.IsNullOrEmpty(propertyText))
            {
                return NoMatchMultiplier;
            }

            if (string.Equals(propertyText, searchQuery, StringComparison.InvariantCultureIgnoreCase))
            {
                return FullMatchMultiplier;
            }
            else if (propertyText.Contains(searchQuery))
            {
                return PartialMatchMultiplier;
            }

            return NoMatchMultiplier;
        }
    }
}