using SearchEngine.Model.Enum;
using System;

namespace SearchEngine.Model.Entities
{
    public class Medium
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public MediumType Type { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }
    }
}