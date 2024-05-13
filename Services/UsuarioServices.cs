using System;
using BibliotecaApi.Services.Interface;
using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;
using BibliotecaApi.Helpers;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaApi.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ILogger _logger;
        private readonly BibliotecaDbContext _context;
        private readonly string _objecto = "Usuario";
        private readonly UserManager<IdentityUser> userManager;
        private readonly string _defaultPassword = "Seguro@123";

        public UsuarioServices(ILogger<UsuarioServices> logger,BibliotecaDbContext dbContext,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = dbContext;
            this.userManager = userManager;
        }

        public async Task<ResultResponse<List<UsuarioDto>>> Usuarios(){
            try
            {
                var usersData = await _context.Users.ToListAsync();

                var usersWithRoles = usersData
                .Select(async user =>
                {
                    var rolesTask = await userManager.GetRolesAsync(user);

                    var userDto = new UsuarioDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = rolesTask.ToList(),
                    };

                    return userDto;
                })
                .Select(task => task.Result)
                .ToList();
                
                return new ResultResponse<List<UsuarioDto>>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Listado(_objecto),
                    Datos = usersWithRoles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<List<UsuarioDto>>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<UsuarioDto>> Usuario(string id){
            try
            {
                var usersData = _context.Users.Find(id);

                if(usersData == null)
                     return new ResultResponse<UsuarioDto>() { Mensaje = Mensajes.NoExiste(_objecto)};

                var roles = await userManager.GetRolesAsync(usersData);
                var userDto = new UsuarioDto
                {
                    Id = usersData.Id,
                    UserName = usersData.UserName,
                    Email = usersData.Email,
                    Roles = roles.ToList()
                };

                return new ResultResponse<UsuarioDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Generado(_objecto),
                    Datos = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<UsuarioDto>(){ Mensaje = Mensajes.ErrorGenerado(ex.Message)};
            }
        }

        public async Task<ResultResponse<UsuarioDto>> CrearUsuario(UsuarioModel model){
            try
            {
               
                ResultResponse<UsuarioDto> result = new ResultResponse<UsuarioDto>();
                var identityUser = new IdentityUser
                {
                    NormalizedUserName =  model.NormalizedUserName,
                    UserName = model.Username,
                    Email = model.Email,
                    NormalizedEmail = model.Email.ToUpper(),
                    LockoutEnabled = false,
                };

                var identityResult = await userManager.CreateAsync(identityUser, _defaultPassword);

                if (identityResult.Succeeded)
                {
                    var roleIdentityResult = await userManager.AddToRolesAsync(identityUser, new List<string>{model.Id_Rol});
                    if (roleIdentityResult.Succeeded){result = await Usuario(identityUser.Id);}
                    if (result.StatusCode == System.Net.HttpStatusCode.OK){result.Mensaje = $"Usuario {identityUser.UserName} creado. Contraseña establecida {_defaultPassword}.";}
                }
                else{
                    result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    foreach (var error in identityResult.Errors)
                    {
                        result.Mensaje += $" {error.Description}";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<UsuarioDto>() { Mensaje = Mensajes.Error("crear",_objecto,ex.Message)};
            }
        }
    
        public async Task<ResultResponse<UsuarioDto>> ActualizarUsuario(string id,UsuarioModel model)
        {
            try
            {
                
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return new ResultResponse<UsuarioDto>() { Mensaje =  Mensajes.NoExiste(_objecto)};
                }

                // Update user properties
                user.NormalizedUserName = model.NormalizedUserName;
                user.UserName = model.Username;
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();

                var updateResult = await userManager.UpdateAsync(user);
                
                if (updateResult.Succeeded)
                {
                    var currentRoles = await userManager.GetRolesAsync(user);
                    var removeRolesResult = await userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (removeRolesResult.Succeeded)
                    {
                        // Add new roles
                        var addRoleResult = await userManager.AddToRolesAsync(user, new List<string>{model.Id_Rol});

                        if (!addRoleResult.Succeeded)
                        {
                            return new ResultResponse<UsuarioDto>() { Mensaje = FormatErrors(addRoleResult.Errors)};
                        }
                    }
                    else
                    {
                        return new ResultResponse<UsuarioDto>() { Mensaje = FormatErrors(removeRolesResult.Errors)};
                    }
                }


                return new ResultResponse<UsuarioDto>()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Mensaje = Mensajes.Editado(_objecto),
                    Datos =  Usuario(id).Result.Datos
                };
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResultResponse<UsuarioDto>() { Mensaje = Mensajes.Error("actualizar",_objecto,ex.Message) };
            }
        }

        string FormatErrors(IEnumerable<IdentityError> errors)
        {
            return string.Join(" ", errors.Select(error => $" {error.Description}"));
        }

    
    }
}
