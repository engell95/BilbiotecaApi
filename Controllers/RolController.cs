using System;
using BibliotecaApi.Models;
using BibliotecaApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BibliotecaApi.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
    public class RolController : Controller
    {
        private readonly IRolServices _rolServices;
        public RolController(IRolServices service){
            _rolServices = service;
        }

        // GET: api/version/rol
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<IActionResult> Roles()
        {
            var result = await _rolServices.Roles();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }
        


    }
}
