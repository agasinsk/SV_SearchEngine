using SearchEngine.Model.Dtos;
using System.Collections.Generic;

namespace SearchEngine.Service.Interface
{
    public interface ISearchService
    {
        IEnumerable<SearchResultDTO> ApplySearch(string searchInput);
    }
}