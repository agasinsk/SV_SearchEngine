using SearchEngine.Model.Entity;
using SearchEngine.Model.Interface;
using SearchEngine.Service.Interface;
using System;

namespace SearchEngine.Service.Implementation.SearchConfiguration
{
    public class SearchConfigurationFactory : ISearchConfigurationFactory
    {
        public ISearchConfiguration GetSearchConfiguration(ISearchable searchable)
        {
            switch (searchable)
            {
                case Building _:
                    return new BuildingSearchConfiguration();

                case Lock _:
                    return new LockSearchConfiguration();

                case Group _:
                    return new GroupSearchConfiguration();

                case Medium _:
                    return new MediumSearchConfiguration();

                default:
                    throw new ArgumentException("Unknown type of searchable item");
            }
        }
    }
}