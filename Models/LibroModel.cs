using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    public class LibroModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MinLength(5, ErrorMessage = "El campo {0} debe tener un mínimo de {1} caracteres")]
        [MaxLength(100,ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
        public int Copias { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Editorial")]
        public int Id_Editorial { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Autor")]
        public int Id_Autor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Fecha de Publicación")]
        public DateTime Fecha_Publicacion { get; set; }
    }
}
