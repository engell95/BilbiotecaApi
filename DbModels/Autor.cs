using System;

namespace BibliotecaApi.DbModels;
public partial class Autor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public bool Estado { get; set; } = true;
}

