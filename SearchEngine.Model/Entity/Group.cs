using SearchEngine.Model.Interface;
using System;

namespace SearchEngine.Model.Entity
{
    public class Group : ISearchable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}