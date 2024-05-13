
namespace BibliotecaApi.Dtos
{
    public class ResultResponse<TData>: BaseResult
    {
        public ResultResponse():base() { Datos = default!; }
        public ResultResponse(TData data): base()
        {
            Datos = data;
        }
        public TData? Datos {get;set;}
    }
}
