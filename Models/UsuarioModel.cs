using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Nombre Completo")]
        public string NormalizedUserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress]
        [DisplayName("Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Rol")]
        public string Id_Rol { get; set; }

    }
}
