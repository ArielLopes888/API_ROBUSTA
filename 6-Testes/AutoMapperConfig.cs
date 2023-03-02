using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.ROBUSTA.ViewModels;
using AutoMapper;
using Domain.Entities;
using Services.DTO;

namespace _6_Testes
{
    public static class AutoMapperConfig
    {
        public static IMapper GetConfiguration()
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>()
                    .ReverseMap();

            });

            return autoMapperConfig.CreateMapper();
        }
    }
}
