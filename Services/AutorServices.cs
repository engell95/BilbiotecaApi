using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;
using BibliotecaApi.Helpers;

namespace BibliotecaApi.Services
{
    public class AutorServices : IAutorServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Autor";

        public AutorServices(ILogger<AutorServices> logger,BibliotecaDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<ResultResponse<List<Autor>>> Autores(){
            try
            {
                var result = await _context.Autores.Where(x => x.Estado).ToListAsync();
                return new ResultResponse<List<Autor>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<Autor>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<Autor>> Autor(int id){
            try
            {
                var data = await BuscarAutorAsync(id);
                if(data == null)
                {
                    return new ResultResponse<Autor>() { Mensaje = Mensajes.NoExiste(_objecto)};
                }

                return new ResultResponse<Autor>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Autor>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<Autor>> CrearAutor(AutorModel autor)
        {
            try
            {
                var data = new Autor() { Nombre = autor.Nombre};
                _context.Autores.Add(data);
                await GuardarCambiosAsync();

                return new ResultResponse<Autor>()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Autor>() { Mensaje = Mensajes.Error("crear",_objecto,ex.Message)};
            }
        }

        public async Task<ResultResponse<Autor>> ActualizarAutor(int id,AutorModel autor)
        {
            try
            {
                
                var data = await BuscarAutorAsync(id);
                if(data == null)
                {
                    return new ResultResponse<Autor>() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Nombre = autor.Nombre;
                await GuardarCambiosAsync();
                return new ResultResponse<Autor>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Editado(_objecto),
                    Datos = data
                };
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Autor>() { Mensaje = Mensajes.Error("actualizar",_objecto,ex.Message) };
            }
        }

        public async Task<BaseResult> EliminarAutor(int id)
        {
            try
            {
                var autor = await BuscarAutorAsync(id);
                if (autor == null)
                {
                    return new BaseResult() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                autor.Estado = false;
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

        private async Task<Autor> BuscarAutorAsync(int id)
        {
            return await _context.Autores.FindAsync(id);
        }

        private async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

    }

    
}
