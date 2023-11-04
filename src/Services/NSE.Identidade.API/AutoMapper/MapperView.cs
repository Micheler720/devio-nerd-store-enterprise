using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NSE.Identidade.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Identidade.API.AutoMapper
{
    public class MapperView : Profile
    {
        public MapperView()
        {
            CreateMap<IdentityUser, UsuarioRegistro>()
                .ForMember(iu => iu.Email, u => u.MapFrom(x => x.Email))
                .ForMember(iu => iu.Email, u => u.MapFrom(x => x.UserName))
                .ReverseMap();
        }
    }
}
