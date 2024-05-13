using System.Net;

namespace BibliotecaApi.Dtos
{
    public class BaseResult
    {
        public HttpStatusCode StatusCode {get;set;}
        public string Mensaje { get; set; }
        public BaseResult(){
            StatusCode = HttpStatusCode.BadRequest;
            Mensaje = string.Empty;
        }
    }
}
