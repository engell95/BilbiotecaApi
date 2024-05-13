using System;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaApi.DbModels
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int Id_Libro { get; set; } 
        public string Id_Usuario { get; set; } 
        public DateTime Fecha_Prestamo { get; set; }
        public DateTime Fecha_Devolucion_Esperada { get; set; }
        public DateTime? Fecha_Devolucion_Real { get; set; } // Puede ser nulo si el libro aún no se ha devuelto
        public Libro Libro { get; set; } 
        public IdentityUser Usuario { get; set; } 
        public bool Estado { get; set; } = true;
    }
}
