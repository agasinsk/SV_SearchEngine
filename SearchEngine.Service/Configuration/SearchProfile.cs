using AutoMapper;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Service.Configuration
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {
            CreateMap<RootObject, IEnumerable<SearchResultDTO>>()
                .ConstructUsing((src, ctx) =>
                {
                    return ctx.Mapper.Map<IEnumerable<SearchResultDTO>>(src.Buildings)
                    .Concat(ctx.Mapper.Map<IEnumerable<SearchResultDTO>>(src.Locks))
                    .Concat(ctx.Mapper.Map<IEnumerable<SearchResultDTO>>(src.Groups))
                    .Concat(ctx.Mapper.Map<IEnumerable<SearchResultDTO>>(src.Media))
                    .ToList();
                });

            CreateMap<Building, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Building))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Shortcut: {src.ShortCut}"));

            CreateMap<Lock, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Lock))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.SerialNumber))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Type: {src.Type} Floor: {src.Floor} Room: {src.RoomNumber}"));

            CreateMap<Group, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Group))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description));

            CreateMap<Medium, SearchResultDTO>()
                .ForMember(dst => dst.SearchObjectType, m => m.MapFrom(src => SearchObjectType.Medium))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.SerialNumber))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Type))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Type: {src.Type} Owner: {src.Owner}"));
        }
    }
}