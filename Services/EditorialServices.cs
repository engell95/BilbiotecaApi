using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;
using BibliotecaApi.Helpers;

namespace BibliotecaApi.Services
{
    public class EditorialServices : IEditorialServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Editorial";

        public EditorialServices(ILogger<EditorialServices> logger,BibliotecaDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<ResultResponse<List<Editorial>>> Editoriales(){
            try
            {
                var result = await _context.Editoriales.Where(x => x.Estado).ToListAsync();
                return new ResultResponse<List<Editorial>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<Editorial>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<Editorial>> Editorial(int id){
            try
            {
                var data = await BuscarEditorialAsync(id);
                if(data == null)
                {
                    return new ResultResponse<Editorial>() { Mensaje = Mensajes.NoExiste(_objecto)};
                }

                return new ResultResponse<Editorial>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Editorial>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<Editorial>> CrearEditorial(EditorialModel editorial)
        {
            try
            {
                var data = new Editorial() { Nombre = editorial.Nombre};
                _context.Editoriales.Add(data);
                await GuardarCambiosAsync();

                return new ResultResponse<Editorial>()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = data
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Editorial>() { Mensaje = Mensajes.Error("crear",_objecto,ex.Message)};
            }
        }

        public async Task<ResultResponse<Editorial>> ActualizarEditorial(int id,EditorialModel editorial)
        {
            try
            {
                
                var data = await BuscarEditorialAsync(id);
                if(data == null)
                {
                    return new ResultResponse<Editorial>() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                data.Nombre = editorial.Nombre;
                await GuardarCambiosAsync();
                return new ResultResponse<Editorial>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Editado(_objecto),
                    Datos = data
                };
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<Editorial>() { Mensaje = Mensajes.Error("actualizar",_objecto,ex.Message) };
            }
        }

        public async Task<BaseResult> EliminarEditorial(int id)
        {
            try
            {
                var data = await BuscarEditorialAsync(id);
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

        private async Task<Editorial> BuscarEditorialAsync(int id)
        {
            return await _context.Editoriales.FindAsync(id);
        }

        private async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
