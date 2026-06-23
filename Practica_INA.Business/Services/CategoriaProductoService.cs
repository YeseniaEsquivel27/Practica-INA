using AutoMapper;
using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Business.Services
{
    public class CategoriaProductoService : ICategoriaProductoService
    {
        private readonly ICategoriaProductoRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaProductoService(
            ICategoriaProductoRepository categoriaRepository,
            IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<CategoriaProductoResponseDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var categorias = await _categoriaRepository.ObtenerTodosAsync();
                var categoriasDto = _mapper.Map<List<CategoriaProductoResponseDTO>>(categorias);

                return new Response<List<CategoriaProductoResponseDTO>>
                {
                    Success = true,
                    Message = "Consulta realizada correctamente.",
                    Data = categoriasDto
                };
            }
            catch (Exception ex)
            {
                return new Response<List<CategoriaProductoResponseDTO>>
                {
                    Success = false,
                    Message = "Error al consultar categorías: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<CategoriaProductoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.ObtenerPorIdAsync(id);

                if (categoria == null)
                {
                    return new Response<CategoriaProductoResponseDTO>
                    {
                        Success = false,
                        Message = "Categoría no encontrada.",
                        Data = null
                    };
                }

                var categoriaDto = _mapper.Map<CategoriaProductoResponseDTO>(categoria);

                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = true,
                    Message = "Consulta realizada correctamente.",
                    Data = categoriaDto
                };
            }
            catch (Exception ex)
            {
                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al consultar la categoría: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<CategoriaProductoResponseDTO>> CrearAsync(CategoriaProductoCreateDTO dto)
        {
            try
            {
                // Validar nombre obligatorio
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    return new Response<CategoriaProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre de la categoría es obligatorio.",
                        Data = null
                    };
                }

                // Validar nombre único
                var categoriaExistente = await _categoriaRepository.ObtenerPorNombreAsync(dto.Nombre);
                if (categoriaExistente != null)
                {
                    return new Response<CategoriaProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre de la categoría ya existe.",
                        Data = null
                    };
                }

                var categoria = _mapper.Map<CategoriaProducto>(dto);
                categoria.Estado = true;
                categoria.FechaCreacion = DateTime.Now;

                await _categoriaRepository.AgregarAsync(categoria);

                var categoriaCreada = await _categoriaRepository.ObtenerPorIdAsync(categoria.Id);
                var categoriaDto = _mapper.Map<CategoriaProductoResponseDTO>(categoriaCreada);

                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = true,
                    Message = "Categoría creada correctamente.",
                    Data = categoriaDto
                };
            }
            catch (Exception ex)
            {
                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al crear la categoría: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<CategoriaProductoResponseDTO>> ActualizarAsync(CategoriaProductoUpdateDTO dto)
        {
            try
            {
                // Validar que existe
                var categoriaExistente = await _categoriaRepository.ObtenerPorIdAsync(dto.Id);
                if (categoriaExistente == null)
                {
                    return new Response<CategoriaProductoResponseDTO>
                    {
                        Success = false,
                        Message = "No se permite actualizar categorías inexistentes.",
                        Data = null
                    };
                }

                // Validar nombre obligatorio
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    return new Response<CategoriaProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre de la categoría es obligatorio.",
                        Data = null
                    };
                }

                // Validar nombre único (si cambió)
                if (categoriaExistente.Nombre != dto.Nombre)
                {
                    var categoriaOtroNombre = await _categoriaRepository.ObtenerPorNombreAsync(dto.Nombre);
                    if (categoriaOtroNombre != null)
                    {
                        return new Response<CategoriaProductoResponseDTO>
                        {
                            Success = false,
                            Message = "El nombre de la categoría ya existe.",
                            Data = null
                        };
                    }
                }

                var categoria = _mapper.Map<CategoriaProducto>(dto);
                await _categoriaRepository.ActualizarAsync(categoria);

                var categoriaActualizada = await _categoriaRepository.ObtenerPorIdAsync(categoria.Id);
                var categoriaDto = _mapper.Map<CategoriaProductoResponseDTO>(categoriaActualizada);

                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = true,
                    Message = "Categoría actualizada correctamente.",
                    Data = categoriaDto
                };
            }
            catch (Exception ex)
            {
                return new Response<CategoriaProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al actualizar la categoría: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<string>> EliminarAsync(int id)
        {
            try
            {
                var categoriaExistente = await _categoriaRepository.ObtenerPorIdAsync(id);
                if (categoriaExistente == null)
                {
                    return new Response<string>
                    {
                        Success = false,
                        Message = "No se permite eliminar categorías inexistentes.",
                        Data = null
                    };
                }

                await _categoriaRepository.EliminarAsync(id);

                return new Response<string>
                {
                    Success = true,
                    Message = "Categoría eliminada correctamente.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Error al eliminar la categoría: " + ex.Message,
                    Data = null
                };
            }
        }
    }
}
