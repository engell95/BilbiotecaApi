using System;

namespace BibliotecaApi.Dtos
{
    public class PrestamoDto
    {
        public int Id { get; set; }
        public DateTime Fecha_Prestamo { get; set; }
        public DateTime Fecha_Devolucion_Esperada { get; set; }
        public DateTime? Fecha_Devolucion_Real { get; set; }
        public LibroDto Libro { get; set; }
        public string Id_Usuario { get; set; }
        public string Usuario { get; set; } = "";
    }
}
