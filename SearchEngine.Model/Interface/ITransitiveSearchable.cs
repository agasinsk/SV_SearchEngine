using System;

namespace SearchEngine.Model.Interface
{
    public interface ITransitiveSearchable : ISearchable
    {
        Guid TransitiveId { get; }
    }
}