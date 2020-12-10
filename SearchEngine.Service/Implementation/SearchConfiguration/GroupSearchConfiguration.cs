using SearchEngine.Model.Entity;
using System.Collections.Generic;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public class GroupSearchConfiguration : BaseSearchConfiguration
    {
        public override string TransitivePropertyName => null;

        protected override IDictionary<string, (int weight, int transitiveWeight)> SearchWeights => new Dictionary<string, (int, int)>
        {
            { nameof(Group.Name), (9, 8) },
            { nameof(Group.Description), (5, 0) },
        };
    }
}