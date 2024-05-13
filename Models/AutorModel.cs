using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    public class AutorModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MinLength(5, ErrorMessage = "El campo {0} debe tener un mínimo de {1} caracteres")]
        [MaxLength(100,ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres")]
        public string Nombre { get; set; }
    }
}
