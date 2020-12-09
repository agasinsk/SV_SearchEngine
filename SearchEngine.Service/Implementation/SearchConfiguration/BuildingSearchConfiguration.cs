using SearchEngine.Model.Entity;
using System.Collections.Generic;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public class BuildingSearchConfiguration : BaseSearchConfiguration
    {
        protected override IDictionary<string, (int weight, int transitiveWeight)> SearchWeights => new Dictionary<string, (int, int)>
        {
            { nameof(Building.Name), (9, 8) },
            { nameof(Building.ShortCut), (7, 5) },
            { nameof(Building.Description), (5, 0) },
        };
    }
}