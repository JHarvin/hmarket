using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Errors;

namespace WebApi.MiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _log;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleWare(RequestDelegate netx, ILogger<ExceptionMiddleWare> log, IHostEnvironment env)
        {
            _next = netx;
            _log = log;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context) {

            try
            {
                await _next(context);
            }
            catch (Exception e)
            {

                _log.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ? new CodeErrorException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                    : new CodeErrorException((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
            }

        }
    }
}
