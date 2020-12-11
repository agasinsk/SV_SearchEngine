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

        public async Task<IEnumerable<SearchResultDTO>> ApplySearch(string query)
        {
            var data = await _dataProvider.GetData();

            if (string.IsNullOrEmpty(query))
            {
                return _mapper.Map<IEnumerable<SearchResultDTO>>(data);
            }

            var searchableItems = data.Buildings.Cast<ISearchable>()
                .Concat(data.Locks)
                .Concat(data.Groups)
                .Concat(data.Media);

            var searchEvaluations = searchableItems.ToDictionary(x => x, v => _searchEvaluator.Evaluate(v, query));

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

        private int GetTotalSearchWeight(ISearchable searchableItem, Dictionary<ISearchable, (int weight, int transitiveWeight)> evaluations)
        {
            switch (searchableItem)
            {
                case ITransitiveSearchable transitiveSearchable:
                    var transitiveItem = evaluations.Keys.FirstOrDefault(x => x.Id == transitiveSearchable.TransitiveId);

                    return searchableItem != null
                        ? evaluations[searchableItem].weight + evaluations[transitiveItem].weight
                        : evaluations[searchableItem].weight;

                case ISearchable searchable:
                    return evaluations[searchableItem].weight;

                default:
                    return default;
            }
        }
    }
}