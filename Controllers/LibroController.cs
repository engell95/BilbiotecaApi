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
    public class LibroController : Controller
    {
        private readonly ILibroServices _libroServices;
        public LibroController(ILibroServices service){
            _libroServices = service;
        }

        // GET: api/version/Libro
        [MapToApiVersion(1)]
        [HttpGet()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> Libros()
        {
            var result = await _libroServices.Libros();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // GET: api/version/Libro/5
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador,Estudiante")]
        public async Task<IActionResult> Libro(int id)
        {
            var result = await _libroServices.Libro(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // POST: api/version/Libro
        [MapToApiVersion(1)]
        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> CrearLibro([FromBody] LibroModel model)
        {
            var result = await _libroServices.CrearLibro(model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // PUT: api/version/Libro/5
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> ActualizarLibro(int id,LibroModel model)
        {
            var result = await _libroServices.ActualizarLibro(id,model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // DELETE: api/version/Libro/5
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrador")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var result = await _libroServices.EliminarLibro(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje });
        }
        
    }
}
