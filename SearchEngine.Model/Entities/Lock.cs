using SearchEngine.Model.Enum;
using System;

namespace SearchEngine.Model.Entities
{
    public class Lock
    {
        public Guid Id { get; set; }

        public Guid BuildingId { get; set; }

        public LockType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public string Floor { get; set; }

        public string RoomNumber { get; set; }
    }
}