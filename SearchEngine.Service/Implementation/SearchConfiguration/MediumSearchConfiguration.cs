using SearchEngine.Model.Entity;
using System.Collections.Generic;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public class MediumSearchConfiguration : BaseSearchConfiguration
    {
        public override string TransitivePropertyName => nameof(Medium.GroupId);

        protected override IDictionary<string, (int weight, int transitiveWeight)> SearchWeights => new Dictionary<string, (int, int)>
        {
            { nameof(Medium.Owner), (10, 0) },
            { nameof(Medium.Type), (3, 0) },
            { nameof(Medium.SerialNumber), (8, 0) },
            { nameof(Medium.Description), (6, 0) },
        };
    }
}