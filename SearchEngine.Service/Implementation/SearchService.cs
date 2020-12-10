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
        private readonly ISearchEvaluator _searchEvaluator;
        private readonly IMapper _mapper;

        public SearchService(IDataProvider<RootObject> dataProvider, IMapper mapper, ISearchEvaluator searchEvaluator)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _searchEvaluator = searchEvaluator;
        }

        public async Task<IEnumerable<SearchResultDTO>> ApplySearch(string searchString)
        {
            var data = await _dataProvider.GetData();

            if (string.IsNullOrEmpty(searchString))
            {
                return _mapper.Map<IEnumerable<SearchResultDTO>>(data);
            }

            var searchableItems = data.Buildings.Cast<ISearchable>()
                .Concat(data.Locks)
                .Concat(data.Groups)
                .Concat(data.Media);

            var searchEvaluations = searchableItems
                .ToDictionary(x => x, v => _searchEvaluator.Evaluate(v, searchString));

            var finalSearchEvaluations = searchEvaluations
                .Select(x => new
                {
                    Item = x.Key,
                    TotalWeight = GetTotalSearchWeight(x.Key, searchEvaluations)
                })
                .OrderByDescending(x => x.TotalWeight);

            var searchResults = _mapper.Map<IEnumerable<SearchResultDTO>>(finalSearchEvaluations.Select(_ => _.Item));

            return searchResults;
        }

        private int GetTotalSearchWeight(ISearchable key, Dictionary<ISearchable, (int weight, int transitiveWeight)> searchEvaluations)
        {
            switch (key)
            {
                case ITransitiveSearchable ts:
                    var searchableItem = searchEvaluations.FirstOrDefault(x => x.Key.Id == ts.TransitiveId);
                    return searchEvaluations[key].weight + searchEvaluations[searchableItem.Key].weight;

                case ISearchable s:
                    return searchEvaluations[key].weight;

                default:
                    return default;
            }
        }
    }
}