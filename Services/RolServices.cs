using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.DbModels;
using Microsoft.AspNetCore.Identity;
using BibliotecaApi.Dtos;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Helpers;

namespace BibliotecaApi.Services
{
    public class RolServices : IRolServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Rol";

        public RolServices(ILogger<RolServices> logger,BibliotecaDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

         public async Task<ResultResponse<List<IdentityRole>>> Roles(){
            try
            {
                var data = await _context.Roles.ToListAsync();
                
                return new ResultResponse<List<IdentityRole>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<IdentityRole>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

    }
}
