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
    
    public class PrestamoController : Controller
    {
        private readonly IPrestamoServices _prestamoServices;
        public PrestamoController(IPrestamoServices service){
            _prestamoServices = service;
        }

        // GET: api/version/Prestamo
        [MapToApiVersion(1)]
        [HttpGet()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> Prestamos()
        {
            var result = await _prestamoServices.Prestamos();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }
        
        // GET: api/version/Prestamo/5
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> Prestamo(int id)
        {
            var result = await _prestamoServices.Prestamo(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // POST: api/version/Prestamo
        [MapToApiVersion(1)]
        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> CrearPrestamo([FromBody] PrestamoModel model)
        {
            var result = await _prestamoServices.CrearPrestamo(model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // PUT: api/version/Prestamo/5
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> ActualizarPrestamo(int id,PrestamoModel model)
        {
            var result = await _prestamoServices.ActualizarPrestamo(id,model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // DELETE: api/version/Prestamo/5
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> EliminarPrestamo(int id)
        {
            var result = await _prestamoServices.EliminarPrestamo(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje });
        }

        // PUT: api/version/Prestamo/5/Devolucion
        [MapToApiVersion(1)]
        [HttpPut("{id}/Devolucion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> DevolverPrestamo(int id)
        {
            var result = await _prestamoServices.DevolverPrestamo(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje });
        }

        // GET: api/version/Prestamo/5/Usuario
        [MapToApiVersion(1)]
        [HttpGet("{id}/Usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Estudiante")]
        public async Task<IActionResult> PrestamosUsuario(string id)
        {
            var result = await _prestamoServices.PrestamosUsuario(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

    }
}
