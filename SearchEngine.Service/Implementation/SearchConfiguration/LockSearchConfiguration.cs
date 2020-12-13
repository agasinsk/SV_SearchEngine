using SearchEngine.Model.Entity;
using System.Collections.Generic;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public class LockSearchConfiguration : BaseSearchConfiguration
    {
        protected override IDictionary<string, (int weight, int transitiveWeight)> SearchWeights => new Dictionary<string, (int, int)>
        {
            { nameof(Lock.Name), (10, 0) },
            { nameof(Lock.Type), (3, 0) },
            { nameof(Lock.SerialNumber), (8, 0) },
            { nameof(Lock.Floor), (6, 0) },
            { nameof(Lock.RoomNumber), (6, 0) },
            { nameof(Lock.Description), (6, 0) },
        };
    }
}