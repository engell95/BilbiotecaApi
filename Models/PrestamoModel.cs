using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BibliotecaApi.Validations;

namespace BibliotecaApi.Models
{
    public class PrestamoModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Libro")]
        public int Id_Libro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Usuario")]
        public string Id_Usuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Fecha Devolucion")]
        [FechaMayorQueHoy(ErrorMessage = "La fecha de Devolucion debe ser mayor que hoy")]
        public DateTime Fecha_Devolucion { get; set; }
    }
}
