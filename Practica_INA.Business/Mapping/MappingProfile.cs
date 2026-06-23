using AutoMapper;
using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoriaProductoCreateDTO, CategoriaProducto>();
            CreateMap<CategoriaProductoUpdateDTO, CategoriaProducto>();
            CreateMap<CategoriaProducto, CategoriaProductoResponseDTO>();

            CreateMap<ProductoCreateDTO, Producto>();
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<Producto, ProductoResponseDTO>()
                .ForMember(dest => dest.CategoriaProductoNombre, 
                    opt => opt.MapFrom(src => src.CategoriaProducto.Nombre));
        }
    }
}
