using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Model.Entity
{
    public class RootObject
    {
        public IEnumerable<Building> Buildings { get; set; } = Enumerable.Empty<Building>();

        public IEnumerable<Lock> Locks { get; set; } = Enumerable.Empty<Lock>();

        public IEnumerable<Group> Groups { get; set; } = Enumerable.Empty<Group>();

        public IEnumerable<Medium> Media { get; set; } = Enumerable.Empty<Medium>();
    }
}