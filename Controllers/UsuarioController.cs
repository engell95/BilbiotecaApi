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
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        public UsuarioController(IUsuarioServices service){
            _usuarioServices = service;
        }

        // GET: api/version/Usuario
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<IActionResult> Usuarios()
        {
            var result = await _usuarioServices.Usuarios();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }
        
        // GET: api/version/Usuario/5
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Usuario(string id)
        {
            var result = await _usuarioServices.Usuario(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // POST: api/version/Usuario
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioModel model)
        {
            var result = await _usuarioServices.CrearUsuario(model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // PUT: api/version/Usuario/5
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(string id,UsuarioModel model)
        {
            var result = await _usuarioServices.ActualizarUsuario(id,model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }
        
    }
}
