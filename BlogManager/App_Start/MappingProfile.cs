using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // to dto
            Mapper.Initialize(cfg => cfg.CreateMap<Entry, EntryDto>());
            Mapper.Initialize(cfg => cfg.CreateMap<Paragraph, ParagraphDto>());

            // from dto
            Mapper.Initialize(cfg => cfg.CreateMap<EntryDto, Entry>());
            Mapper.Initialize(cfg => cfg.CreateMap<ParagraphDto, Paragraph>());
        }
    }
}