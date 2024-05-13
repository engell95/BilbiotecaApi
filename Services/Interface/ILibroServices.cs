using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services.Interface
{
    public interface ILibroServices
    {
        Task<ResultResponse<List<LibroDto>>> Libros();
        Task<ResultResponse<LibroDto>> Libro(int id);
        Task<ResultResponse<LibroDto>> CrearLibro(LibroModel libro);
        Task<ResultResponse<LibroDto>> ActualizarLibro(int id,LibroModel libro);
        Task<BaseResult> EliminarLibro(int id);
    }
}
