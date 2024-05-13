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
    public class AutorController: Controller
    {
        private readonly IAutorServices _autorServices;

        public AutorController(IAutorServices service){
            _autorServices = service;
        }

        // GET: api/version/Autor
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<IActionResult> Autores()
        {
            var result = await _autorServices.Autores();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // GET: api/version/Autor/5
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Autor(int id)
        {
            var result = await _autorServices.Autor(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }
        
        // POST: api/version/Autor
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<IActionResult> CrearAutor([FromBody] AutorModel model)
        {
            var result = await _autorServices.CrearAutor(model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // PUT: api/version/Autor/5
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAutor(int id,AutorModel model)
        {
            var result = await _autorServices.ActualizarAutor(id,model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // DELETE: api/version/Autor/5
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAutor(int id)
        {
            var result = await _autorServices.EliminarAutor(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje });
        }

     

    }
}
