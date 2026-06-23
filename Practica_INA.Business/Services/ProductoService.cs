using AutoMapper;
using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Business.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaProductoRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public ProductoService(
            IProductoRepository productoRepository,
            ICategoriaProductoRepository categoriaRepository,
            IMapper mapper)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<ProductoResponseDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var productos = await _productoRepository.ObtenerTodosAsync();
                var productosDto = _mapper.Map<List<ProductoResponseDTO>>(productos);

                return new Response<List<ProductoResponseDTO>>
                {
                    Success = true,
                    Message = "Consulta realizada correctamente.",
                    Data = productosDto
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ProductoResponseDTO>>
                {
                    Success = false,
                    Message = "Error al consultar productos: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<ProductoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(id);

                if (producto == null)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "Producto no encontrado.",
                        Data = null
                    };
                }

                var productoDto = _mapper.Map<ProductoResponseDTO>(producto);

                return new Response<ProductoResponseDTO>
                {
                    Success = true,
                    Message = "Consulta realizada correctamente.",
                    Data = productoDto
                };
            }
            catch (Exception ex)
            {
                return new Response<ProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al consultar el producto: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<ProductoResponseDTO>> CrearAsync(ProductoCreateDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre del producto es obligatorio.",
                        Data = null
                    };
                }

                if (dto.Precio <= 0)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El precio debe ser mayor que cero.",
                        Data = null
                    };
                }

                if (dto.Stock <= 0)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El stock debe ser mayor que cero.",
                        Data = null
                    };
                }

                var productoExistente = await _productoRepository.ObtenerPorNombreAsync(dto.Nombre);
                if (productoExistente != null)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre del producto ya existe.",
                        Data = null
                    };
                }

                var categoriaExiste = await _categoriaRepository.ExisteAsync(dto.CategoriaProductoId);
                if (!categoriaExiste)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "La categoría especificada no existe.",
                        Data = null
                    };
                }

                var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaProductoId);
                if (!categoria.Estado)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "No se pueden crear productos con categorías inactivas.",
                        Data = null
                    };
                }

                var producto = _mapper.Map<Producto>(dto);
                producto.Estado = true;
                producto.FechaCreacion = DateTime.Now;

                await _productoRepository.AgregarAsync(producto);

                var productoCreado = await _productoRepository.ObtenerPorIdAsync(producto.Id);
                var productoDto = _mapper.Map<ProductoResponseDTO>(productoCreado);

                return new Response<ProductoResponseDTO>
                {
                    Success = true,
                    Message = "Producto creado correctamente.",
                    Data = productoDto
                };
            }
            catch (Exception ex)
            {
                return new Response<ProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al crear el producto: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO dto)
        {
            try
            {
                var productoExistente = await _productoRepository.ObtenerPorIdAsync(dto.Id);
                if (productoExistente == null)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "No se permite actualizar productos inexistentes.",
                        Data = null
                    };
                }

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El nombre del producto es obligatorio.",
                        Data = null
                    };
                }

                if (dto.Precio <= 0)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El precio debe ser mayor que cero.",
                        Data = null
                    };
                }

                if (dto.Stock <= 0)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "El stock debe ser mayor que cero.",
                        Data = null
                    };
                }

                if (productoExistente.Nombre != dto.Nombre)
                {
                    var productoOtroNombre = await _productoRepository.ObtenerPorNombreAsync(dto.Nombre);
                    if (productoOtroNombre != null)
                    {
                        return new Response<ProductoResponseDTO>
                        {
                            Success = false,
                            Message = "El nombre del producto ya existe.",
                            Data = null
                        };
                    }
                }

                var categoriaExiste = await _categoriaRepository.ExisteAsync(dto.CategoriaProductoId);
                if (!categoriaExiste)
                {
                    return new Response<ProductoResponseDTO>
                    {
                        Success = false,
                        Message = "No se permite asignar una categoría inexistente al actualizar.",
                        Data = null
                    };
                }

                var producto = _mapper.Map<Producto>(dto);
                await _productoRepository.ActualizarAsync(producto);

                var productoActualizado = await _productoRepository.ObtenerPorIdAsync(producto.Id);
                var productoDto = _mapper.Map<ProductoResponseDTO>(productoActualizado);

                return new Response<ProductoResponseDTO>
                {
                    Success = true,
                    Message = "Producto actualizado correctamente.",
                    Data = productoDto
                };
            }
            catch (Exception ex)
            {
                return new Response<ProductoResponseDTO>
                {
                    Success = false,
                    Message = "Error al actualizar el producto: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Response<string>> EliminarAsync(int id)
        {
            try
            {
                var productoExistente = await _productoRepository.ObtenerPorIdAsync(id);
                if (productoExistente == null)
                {
                    return new Response<string>
                    {
                        Success = false,
                        Message = "Producto no encontrado.",
                        Data = null
                    };
                }

                await _productoRepository.EliminarAsync(id);

                return new Response<string>
                {
                    Success = true,
                    Message = "Producto eliminado correctamente.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "Error al eliminar el producto: " + ex.Message,
                    Data = null
                };
            }
        }
    }
}
