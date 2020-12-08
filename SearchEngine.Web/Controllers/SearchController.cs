using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResultDTO>> Search(string searchString)
        {
            return await _searchService.ApplySearch(searchString);
        }
    }
}