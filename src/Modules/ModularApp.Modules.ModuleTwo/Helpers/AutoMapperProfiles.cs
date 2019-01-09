using AutoMapper;
using ModularApp.Modules.ModuleTwo.Dto;
using ModularApp.Modules.ModuleTwo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularApp.Modules.ModuleTwo.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Sale, SaleDto>();
        }
    }
}
