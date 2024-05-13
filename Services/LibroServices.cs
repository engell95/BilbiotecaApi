using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;
using BibliotecaApi.Helpers;

namespace BibliotecaApi.Services
{
    public class LibroServices : ILibroServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Libro";

        public LibroServices(ILogger<LibroServices> logger,BibliotecaDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<ResultResponse<List<LibroDto>>> Libros(){
            try
            {
                var result = await _context.Libros.Where(x => x.Estado)
                .Include(x => x.Autor) 
                .Include(x => x.Editorial) 
                .Select(x => 
                    new LibroDto { 
                        Id = x.Id,
                        Nombre = x.Nombre, 
                        Descripcion =x.Descripcion,
                        Copias = x.Copias,
                        Fecha_Publicacion = x.Fecha_Publicacion,
                        Id_Autor = x.Autor.Id, 
                        Autor = x.Autor.Nombre, 
                        Id_Editorial = x.Editorial.Id,
                        Editorial = x.Editorial.Nombre 
                    }
                )
                .ToListAsync();
                
                return new ResultResponse<List<LibroDto>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<LibroDto>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<LibroDto>> Libro(int id){
            try
            {
                var data = await _context.Libros.Where(x => x.Id == id)
                .Include(x => x.Autor) 
                .Include(x => x.Editorial) 
                .Select(x => 
                    new LibroDto { 
                        Id = x.Id,
                        Nombre = x.Nombre, 
                        Descripcion =x.Descripcion,
                        Copias = x.Copias,
                        Fecha_Publicacion = x.Fecha_Publicacion,
                        Id_Autor = x.Autor.Id, 
                        Autor = x.Autor.Nombre, 
                        Id_Editorial = x.Editorial.Id,
                        Editorial = x.Editorial.Nombre 
                    }
                )
                .FirstOrDefaultAsync();

                if(data == null)
                {
                    return new ResultResponse<LibroDto>() { Mensaje = Mensajes.NoExiste(_objecto)};
                }

                return new ResultResponse<LibroDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<LibroDto>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }
        
        public async Task<ResultResponse<LibroDto>> CrearLibro(LibroModel libro)
        {
            try
            {
                var data = new Libro() { 
                    Nombre = libro.Nombre,
                    Descripcion = libro.Descripcion,
                    Copias = libro.Copias,
                    Fecha_Publicacion = libro.Fecha_Publicacion,
                    Id_Autor = libro.Id_Autor,
                    Id_Editorial = libro.Id_Editorial,
                    Estado = true
                };
                _context.Libros.Add(data);
                await GuardarCambiosAsync();

                return new ResultResponse<LibroDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = Libro(data.Id).Result.Datos
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<LibroDto>() { Mensaje = Mensajes.Error("crear",_objecto,ex.Message)};
            }
        }
        
        public async Task<ResultResponse<LibroDto>> ActualizarLibro(int id,LibroModel libro)
        {
            try
            {
                
                var data = await BuscarLibroAsync(id);
                if(data == null)
                {
                    return new ResultResponse<LibroDto>() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Nombre = libro.Nombre;
                data.Descripcion = libro.Descripcion;
                data.Fecha_Publicacion = libro.Fecha_Publicacion;
                data.Id_Autor = libro.Id_Autor;
                data.Id_Editorial = libro.Id_Editorial;
                data.Copias = libro.Copias;
                await GuardarCambiosAsync();
                return new ResultResponse<LibroDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Editado(_objecto),
                    Datos = Libro(data.Id).Result.Datos
                };
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<LibroDto>() { Mensaje = Mensajes.Error("actualizar",_objecto,ex.Message) };
            }
        }

        public async Task<BaseResult> EliminarLibro(int id)
        {
            try
            {
                var data = await BuscarLibroAsync(id);
                if (data == null)
                {
                    return new BaseResult() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Estado = false;
                await GuardarCambiosAsync();
                return new BaseResult()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Eliminado(_objecto)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseResult() { Mensaje = Mensajes.Error("eliminar",_objecto,ex.Message) };
            }
        }

        private async Task<Libro> BuscarLibroAsync(int id)
        {
            return await _context.Libros.FindAsync(id);
        }
        
        private async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
