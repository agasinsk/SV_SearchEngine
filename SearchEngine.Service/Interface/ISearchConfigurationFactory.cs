using SearchEngine.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Service.Interface
{
    public interface ISearchConfigurationFactory
    {
        ISearchConfiguration GetSearchConfiguration(ISearchable searchable);
    }
}