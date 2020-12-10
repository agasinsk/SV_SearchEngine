using SearchEngine.Model.Enum;
using SearchEngine.Model.Interface;
using System;

namespace SearchEngine.Model.Entity
{
    public class Lock : ITransitiveSearchable
    {
        public Guid Id { get; set; }

        public Guid BuildingId { get; set; }

        public LockType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public string Floor { get; set; }

        public string RoomNumber { get; set; }

        public Guid TransitiveId => BuildingId;
    }
}