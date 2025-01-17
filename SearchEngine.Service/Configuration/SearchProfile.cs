﻿using AutoMapper;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using SearchEngine.Service.Extensions;
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
                .ForMember(dst => dst.ResultObjectType, m => m.MapFrom(src => ObjectType.Building))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.Ignore())
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Shortcut: {src.ShortCut}"));

            CreateMap<Lock, SearchResultDTO>()
                .ForMember(dst => dst.ResultObjectType, m => m.MapFrom(src => ObjectType.Lock))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.SerialNumber))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Type: {src.Type.ToString().SplitPascalCase()} • Floor: {src.Floor} • Room: {src.RoomNumber}"));

            CreateMap<Group, SearchResultDTO>()
                .ForMember(dst => dst.ResultObjectType, m => m.MapFrom(src => ObjectType.Group))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.Ignore())
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description));

            CreateMap<Medium, SearchResultDTO>()
                .ForMember(dst => dst.ResultObjectType, m => m.MapFrom(src => ObjectType.Medium))
                .ForMember(dst => dst.ResultObjectId, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.ResultObjectKey, m => m.MapFrom(src => src.SerialNumber))
                .ForMember(dst => dst.ResultObjectName, m => m.MapFrom(src => src.Type.ToString().SplitPascalCase()))
                .ForMember(dst => dst.ResultObjectDescription, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.ResultObjectText, m => m.MapFrom(src => $"Type: {src.Type.ToString().SplitPascalCase()} • Owner: {src.Owner}"));
        }
    }
}