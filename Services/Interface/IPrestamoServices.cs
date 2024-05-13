using BibliotecaApi.Dtos;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interface
{
    public interface IPrestamoServices
    {
        Task<ResultResponse<List<PrestamoDto>>> Prestamos();
        Task<ResultResponse<PrestamoDto>> Prestamo(int id);
        Task<ResultResponse<PrestamoDto>> CrearPrestamo(PrestamoModel prestamo);
        Task<ResultResponse<PrestamoDto>> ActualizarPrestamo(int id,PrestamoModel prestamo);
        Task<BaseResult> EliminarPrestamo(int id);
        Task<BaseResult> DevolverPrestamo(int id);
        Task<ResultResponse<List<PrestamoDto>>> PrestamosUsuario(string id);
    }
}
