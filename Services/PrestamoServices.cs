using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;
using BibliotecaApi.Helpers;

namespace BibliotecaApi.Services
{
    public class PrestamoServices : IPrestamoServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Prestamo";
        private readonly ILibroServices _libroServices;

        public PrestamoServices(ILogger<PrestamoServices> logger,BibliotecaDbContext dbContext,ILibroServices service)
        {
            _logger = logger;
            _context = dbContext;
            _libroServices = service;
        }

        public async Task<ResultResponse<List<PrestamoDto>>> Prestamos(){
            try
            {
                   
                var result = await _context.Prestamos.Where(x => x.Estado) 
                .Include(x => x.Usuario) 
                .Select(x => 
                    new PrestamoDto { 
                        Id = x.Id,
                        Fecha_Prestamo = x.Fecha_Prestamo, 
                        Fecha_Devolucion_Esperada = x.Fecha_Devolucion_Esperada,
                        Fecha_Devolucion_Real = x.Fecha_Devolucion_Real,
                        Libro = _libroServices.Libro(x.Id_Libro).Result.Datos,
                        Id_Usuario = x.Usuario.Id,
                        Usuario = x.Usuario.NormalizedUserName
                    }
                )
                .ToListAsync();
                
                return new ResultResponse<List<PrestamoDto>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<PrestamoDto>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<PrestamoDto>> Prestamo(int id){
            try
            {
                var data = await _context.Prestamos.Where(x => x.Id == id)
                .Include(x => x.Usuario) 
                .Select(x => 
                    new PrestamoDto { 
                        Id = x.Id,
                        Fecha_Prestamo = x.Fecha_Prestamo, 
                        Fecha_Devolucion_Esperada = x.Fecha_Devolucion_Esperada,
                        Fecha_Devolucion_Real = x.Fecha_Devolucion_Real,
                        Libro = _libroServices.Libro(x.Id_Libro).Result.Datos,
                        Id_Usuario = x.Usuario.Id,
                        Usuario = x.Usuario.NormalizedUserName
                    }
                )
                .FirstOrDefaultAsync();

                if(data == null)
                {
                    return new ResultResponse<PrestamoDto>() { Mensaje = Mensajes.NoExiste(_objecto)};
                }

                return new ResultResponse<PrestamoDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<PrestamoDto>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }
        
        public async Task<ResultResponse<PrestamoDto>> CrearPrestamo(PrestamoModel prestamo)
        {
            try
            {
                // Validar si el libro existe
                var libro = await BuscarLibroAsync(prestamo.Id_Libro);
                if(!ValidarPrestamo(libro,prestamo.Id_Usuario,out ResultResponse<PrestamoDto> response))
                    return response;
             
                var data = new Prestamo() { 
                    Id_Libro = prestamo.Id_Libro,
                    Id_Usuario = prestamo.Id_Usuario,
                    Fecha_Prestamo = DateTime.Now,
                    Fecha_Devolucion_Esperada = prestamo.Fecha_Devolucion,
                    Fecha_Devolucion_Real = null,
                    Estado = true
                };

                _context.Prestamos.Add(data);

                //Actualizamos existencia de copias
                libro.Copias = libro.Copias - 1;
                await GuardarCambiosAsync();
                return new ResultResponse<PrestamoDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = Prestamo(data.Id).Result.Datos
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<PrestamoDto>() { Mensaje = Mensajes.Error("crear",_objecto,ex.Message)};
            }
        }

        public async Task<ResultResponse<PrestamoDto>> ActualizarPrestamo(int id,PrestamoModel prestamo)
        {
            try
            {
                
                var data = await BuscarPrestamoAsync(id);
                if(data == null)
                {
                    return new ResultResponse<PrestamoDto>() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Id_Libro = prestamo.Id_Libro;
                data.Id_Usuario = prestamo.Id_Usuario;
                data.Fecha_Devolucion_Esperada = prestamo.Fecha_Devolucion;
                await GuardarCambiosAsync();
                return new ResultResponse<PrestamoDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Editado(_objecto),
                    Datos = Prestamo(data.Id).Result.Datos
                };
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<PrestamoDto>() { Mensaje = Mensajes.Error("actualizar",_objecto,ex.Message) };
            }
        }

        public async Task<BaseResult> EliminarPrestamo(int id)
        {
            try
            {
                var data = await BuscarPrestamoAsync(id);
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

        public async Task<BaseResult> DevolverPrestamo(int id)
        {
            try
            {
                
                var data = await BuscarPrestamoAsync(id);
                if(data == null)
                {
                    return new BaseResult() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Fecha_Devolucion_Real = DateTime.Now;

                //Actualizamos existencia de copias
                var libro = await BuscarLibroAsync(data.Id_Libro);
                libro.Copias = libro.Copias + 1;

                await GuardarCambiosAsync();
                return new BaseResult() { StatusCode = System.Net.HttpStatusCode.OK,Mensaje = "Libro devuelto."};
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseResult() { Mensaje = Mensajes.Error("devolver",_objecto,ex.Message) };
            }
        }

        private bool ValidarPrestamo(Libro? model,string Id_Usuario,out ResultResponse<PrestamoDto> response)
        {
            response = new ResultResponse<PrestamoDto>();

            // Validar si existe un libro
            if (model == null )
            {
                response = new ResultResponse<PrestamoDto>()
                {
                    Mensaje = "El libro especificado no existe."
                };
                return false;
            }

            // Validar si hay copias disponibles del libro
            if (model.Copias == 0)
            {
                response = new ResultResponse<PrestamoDto>()
                {
                    Mensaje = "No hay copias disponibles de este libro."
                };
                return false;
            }
            
            // Validar prestar el mismo libro
            if (_context.Prestamos.Any(x => x.Id_Usuario == Id_Usuario && x.Id_Libro == model.Id && x.Fecha_Devolucion_Real == null && x.Estado))
            {
                response = new ResultResponse<PrestamoDto>()
                {
                    Mensaje = "El libro esta pendiente de devolución por el usuario."
                };
                return false;
            }

            return true;
        }

        public async Task<ResultResponse<List<PrestamoDto>>> PrestamosUsuario(string id){
            try
            {
                   
                var result = await _context.Prestamos.Where(x => x.Estado && x.Id_Usuario == id) 
                .Include(x => x.Usuario) 
                .Select(x => 
                    new PrestamoDto { 
                        Id = x.Id,
                        Fecha_Prestamo = x.Fecha_Prestamo, 
                        Fecha_Devolucion_Esperada = x.Fecha_Devolucion_Esperada,
                        Fecha_Devolucion_Real = x.Fecha_Devolucion_Real,
                        Libro = _libroServices.Libro(x.Id_Libro).Result.Datos,
                        Id_Usuario = x.Usuario.Id,
                        Usuario = x.Usuario.NormalizedUserName
                    }
                )
                .ToListAsync();
                
                return new ResultResponse<List<PrestamoDto>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<PrestamoDto>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        private async Task<Libro> BuscarLibroAsync(int id)
        {
            return await _context.Libros.FindAsync(id);
        }

        private async Task<Prestamo> BuscarPrestamoAsync(int id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        private async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
        
    }
}
