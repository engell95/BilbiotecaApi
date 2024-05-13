using BibliotecaApi.Dtos;
using BibliotecaApi.DbModels;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaApi.Services.Interface
{
    public interface IRolServices
    {
        Task<ResultResponse<List<IdentityRole>>> Roles();
    }
}
