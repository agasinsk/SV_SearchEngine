using SearchEngine.Model.Entity;
using SearchEngine.Model.Interface;
using SearchEngine.Service.Implementation.SearchConfiguration;
using SearchEngine.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Service.Implementation
{
    public class SearchEvaluator
    {
        public (int weight, int transitiveWeight) Evaluate(ISearchable searchableItem, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return (0, 0);
            }

            var searchConfiguration = GetSearchConfiguration(searchableItem);
            var properties = searchableItem.GetType().GetProperties();

            int totalWeight = 0, totalTransitiveWeight = 0;

            foreach (var property in properties)
            {
                var (weight, transitiveWeight) = searchConfiguration.GetWeightsForProperty(property.Name);

                if (weight <= 0 && transitiveWeight <= 0)
                {
                    continue;
                }

                var propertyValue = property.GetValue(searchableItem);

                if (propertyValue != null && propertyValue.ToString().ToLower().Contains(searchString.ToLower()))
                {
                    totalWeight += weight;
                    totalTransitiveWeight += transitiveWeight;
                }
            }

            return (totalWeight, totalTransitiveWeight);
        }

        private ISearchConfiguration GetSearchConfiguration(ISearchable searchableItem)
        {
            switch (searchableItem)
            {
                case Building _:
                    return new BuildingSearchConfiguration();

                case Lock _:
                    return new LockSearchConfiguration();

                case Group _:
                    return new GroupSearchConfiguration();

                case Medium _:
                    return new MediumSearchConfiguration();

                default:
                    throw new ArgumentException("Unknown type of search item");
            }
        }
    }
}