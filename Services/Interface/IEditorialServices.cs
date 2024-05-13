using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interface
{
    public interface IEditorialServices
    {
        Task<ResultResponse<List<Editorial>>> Editoriales();
        Task<ResultResponse<Editorial>> Editorial(int id);
        Task<ResultResponse<Editorial>> CrearEditorial(EditorialModel editorial);
        Task<ResultResponse<Editorial>> ActualizarEditorial(int id,EditorialModel editorial);
        Task<BaseResult> EliminarEditorial(int id);
    }
}
