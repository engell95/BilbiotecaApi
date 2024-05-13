using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using BibliotecaApi.Dtos;
using BibliotecaApi.Models;

namespace BibliotecaApi.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials model) 
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var results = await signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);

                if (results.Succeeded){
                    return await GenerateToken(user);
                }

                if (results.IsLockedOut)
                {
                    return Unauthorized("Cuenta bloqueada contacte al administrador.");
                }
            }

            return Unauthorized("Validar credenciales.");
        }

        [HttpGet("RefreshToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        private async Task<ActionResult<AuthenticationResponse>> Refresh() 
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await userManager.FindByIdAsync(userId); 
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return await GenerateToken(user);
        }

        private async Task<AuthenticationResponse> GenerateToken(IdentityUser model) 
        {
            string userName = model.UserName ?? "testUser";
            string mail = model.Email ?? "testUser";
            string id = model.Id ?? "testUser";
            var roles = await userManager.GetRolesAsync(model);
            var claims = new List<Claim>()
            {
                new Claim("userName", userName),
                new Claim("email", mail),
                new Claim("idUser", id)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            string config = configuration["jwtKey"]?? "";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);
            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }

    }
}
