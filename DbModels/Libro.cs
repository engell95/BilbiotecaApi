using System;

namespace BibliotecaApi.DbModels;
public partial class Libro
{   
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int Copias { get; set; }
    public DateTime Fecha_Publicacion { get; set; }
    public bool Estado { get; set; } = true;
    
    public int Id_Editorial { get; set; }
    public int Id_Autor { get; set; }

    public Editorial Editorial { get; set; }
    public Autor Autor { get; set; }
}