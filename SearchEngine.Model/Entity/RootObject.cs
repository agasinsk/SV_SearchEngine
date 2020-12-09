using System.Collections.Generic;

namespace SearchEngine.Model.Entity
{
    public class RootObject
    {
        public IEnumerable<Building> Buildings { get; set; }

        public IEnumerable<Lock> Locks { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<Medium> Media { get; set; }
    }
}