using AutoMapper;
using Core.Entities;

namespace WebApi.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
        CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Direccion, DireccionDto>().ReverseMap();


        }
    }
}
