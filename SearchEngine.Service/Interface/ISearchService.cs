using SearchEngine.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Service.Interface
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDTO>> ApplySearch(string searchInput);
    }
}