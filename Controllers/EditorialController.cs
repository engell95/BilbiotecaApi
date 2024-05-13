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
    public class EditorialController : Controller
    {
        private readonly IEditorialServices _editorialServices;

        public EditorialController(IEditorialServices service){
            _editorialServices = service;
        }

        // GET: api/version/Editorial
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<IActionResult> Editoriales()
        {
            var result = await _editorialServices.Editoriales();
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // GET: api/version/Editorial/5
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Editorial(int id)
        {
            var result = await _editorialServices.Editorial(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // POST: api/version/Editorial
        [MapToApiVersion(1)]
        [HttpPost()]
        public async Task<IActionResult> CrearEditorial([FromBody] EditorialModel model)
        {
            var result = await _editorialServices.CrearEditorial(model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // PUT: api/version/Editorial/5
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEditorial(int id,EditorialModel model)
        {
            var result = await _editorialServices.ActualizarEditorial(id,model);
            return StatusCode((int)result.StatusCode, new { result.Mensaje, result.Datos });
        }

        // DELETE: api/version/Editorial/5
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEditorial(int id)
        {
            var result = await _editorialServices.EliminarEditorial(id);
            return StatusCode((int)result.StatusCode, new { result.Mensaje });
        }
        
    }
}
