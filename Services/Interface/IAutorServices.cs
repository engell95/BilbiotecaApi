using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interface
{
    public interface IAutorServices
    {
        Task<ResultResponse<List<Autor>>> Autores();
        Task<ResultResponse<Autor>> Autor(int id);
        Task<ResultResponse<Autor>> CrearAutor(AutorModel autor);
        Task<ResultResponse<Autor>> ActualizarAutor(int id,AutorModel autor);
        Task<BaseResult> EliminarAutor(int id);
    }
}
