using SearchEngine.Model.Enum;
using System;

namespace SearchEngine.Model.DTO
{
    public class SearchResultDTO
    {
        public SearchObjectType SearchObjectType { get; set; }

        public int SearchRank { get; set; }

        public Guid ResultObjectId { get; set; }

        public string ResultObjectKey { get; set; }

        public string ResultObjectName { get; set; }

        public string ResultObjectDescription { get; set; }

        public string ResultObjectText { get; set; }
    }
}