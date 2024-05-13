using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interface
{
    public interface IUsuarioServices
    {
        Task<ResultResponse<List<UsuarioDto>>> Usuarios();
        Task<ResultResponse<UsuarioDto>> Usuario(string id);
        Task<ResultResponse<UsuarioDto>> CrearUsuario(UsuarioModel model);
        Task<ResultResponse<UsuarioDto>> ActualizarUsuario(string id,UsuarioModel model);
    }
}
