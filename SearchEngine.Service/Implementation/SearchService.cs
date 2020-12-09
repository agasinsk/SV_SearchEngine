using AutoMapper;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Interface;
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

            var searchableItems = data.Buildings.Cast<ISearchable>()
                .Concat(data.Locks)
                .Concat(data.Groups)
                .Concat(data.Media);
            var searchEvaluator = new SearchEvaluator();

            var searchWeights = searchableItems
                .Select(x => new { x, Weights = searchEvaluator.Evaluate(x, searchInput) })
                .ToList();

            var transitiveWeights = searchWeights
                .Where(x => x.Weights.transitiveWeight > 0)
                .ToDictionary(k => k.x, v => v.Weights.transitiveWeight);

            var searchResults = _mapper.Map<IEnumerable<SearchResultDTO>>(searchWeights.Select(_ => _.x));

            return searchResults;
        }
    }
}