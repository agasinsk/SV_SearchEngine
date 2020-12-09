using AutoMapper;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Service.Implementation
{
    public class SearchService : ISearchService
    {
        private readonly IDataProvider<RootObject> _dataProvider;
        private readonly IMapper _mapper;

        public SearchService(IDataProvider<RootObject> dataProvider, IMapper mapper)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<SearchResultDTO>> ApplySearch(string searchInput)
        {
            var data = await _dataProvider.GetData();

            if (string.IsNullOrEmpty(searchInput))
            {
                return _mapper.Map<IEnumerable<SearchResultDTO>>(data);
            }

            return Enumerable.Empty<SearchResultDTO>();
        }
    }
}