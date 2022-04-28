using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKey, out var extractedApiKey)) 
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Api key não encontrada!"
                };
                return;
            }
            if (!Configuration.ApiKey.Equals(extractedApiKey)) 
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Acesso não autorizado"
                };
                return;
            }
            await next();
        }
    }
}
    