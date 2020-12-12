using Microsoft.AspNetCore.Mvc;
using SearchEngine.Model.DTO;
using SearchEngine.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchEngine.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResultDTO>> Search(string query)
        {
            return await _searchService.ApplySearch(query);
        }
    }
}