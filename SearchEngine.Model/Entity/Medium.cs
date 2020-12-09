using SearchEngine.Model.Enum;
using SearchEngine.Model.Interface;
using System;

namespace SearchEngine.Model.Entity
{
    public class Medium : ISearchable
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public MediumType Type { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }
    }
}