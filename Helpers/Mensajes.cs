using System;

namespace BibliotecaApi.Helpers
{
    public static class Mensajes
    {
        public static string Listado(string objeto)
        {
            return "Datos Listados.";
        }

        public static string Generado(string objeto)
        {
            return "Dato Generado.";
        }

        public static string Creado(string objeto)
        {
            return $"{objeto} creado correctamente.";
        }
        
        public static string NoExiste(string objeto)
        {
            return $"No existe el {objeto}.";
        }

        public static string Error(string accion ,string objeto, string error)
        {
            return $"Error al {accion} {objeto}: {error}.";
        }

        public static string ErrorGenerado(string error)
        {
            return $"Error al generar la información: {error}.";
        }

        public static string Editado(string objeto)
        {
            return $"{objeto} actualizado correctamente.";
        }

        public static string Eliminado(string objeto)
        {
            return $"{objeto} eliminado correctamente.";
        }
    
    }
}
