#region using
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FlyMosquito.Common;
#endregion

namespace FlyMosquito.Extension.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment IHostingEnvironment;

        public GlobalExceptionFilter(Microsoft.AspNetCore.Hosting.IHostingEnvironment iHostingEnvironment)
        {
            IHostingEnvironment = iHostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "发生意外错误！",
                Detail = IHostingEnvironment.IsDevelopment() ? context.Exception.StackTrace : "处理您的请求时发生错误。"
            };
            LoggerHelper.Error(context.Exception.ToString());
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
