using System;

namespace SearchEngine.Model.Interface
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}