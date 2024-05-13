using Microsoft.AspNetCore.Mvc.Filters;

namespace BibliotecaApi.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilter> logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
