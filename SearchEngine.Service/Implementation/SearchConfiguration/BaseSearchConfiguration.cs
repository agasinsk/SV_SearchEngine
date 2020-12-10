using SearchEngine.Service.Interface;
using System.Collections.Generic;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public abstract class BaseSearchConfiguration : ISearchConfiguration
    {
        public abstract string TransitivePropertyName { get; }

        protected abstract IDictionary<string, (int weight, int transitiveWeight)> SearchWeights { get; }

        public int GetTransitiveWeightForProperty(string propertyName)
        {
            return SearchWeights.TryGetValue(propertyName, out var weights) ? weights.weight : default;
        }

        public int GetWeightForProperty(string propertyName)
        {
            return SearchWeights.TryGetValue(propertyName, out var weights) ? weights.transitiveWeight : default;
        }

        public (int weight, int transitiveWeight) GetWeightsForProperty(string propertyName)
        {
            return SearchWeights.TryGetValue(propertyName, out var weights) ? weights : (0, 0);
        }
    }
}