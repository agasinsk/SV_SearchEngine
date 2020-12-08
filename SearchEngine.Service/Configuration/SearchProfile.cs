using AutoMapper;
using SearchEngine.Model;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;

namespace SearchEngine.Service.Configuration
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {
            CreateMap<RootObject, SearchResultDTO>();

            CreateMap<Building, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Building));

            CreateMap<Lock, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Lock));

            CreateMap<Group, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Group));

            CreateMap<Medium, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Medium));
        }
    }
}