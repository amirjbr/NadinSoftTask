using AutoMapper;
using NadinSoftTask.Domain.Entities;
using NadinSoftTask.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadinSoftTask.Application.MapConfig
{
    public class MapConfigs : Profile
    {
        public MapConfigs()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductUpdateDTO>().ReverseMap();
        }
    }
}
