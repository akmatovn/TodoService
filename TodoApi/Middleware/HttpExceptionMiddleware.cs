using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;
using TodoApiDTO.Models;

namespace TodoApiDTO.Middleware
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                    throw;

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var res = JsonConvert.SerializeObject(new BaseResponse<string>
                {
                    Code = -2,
                    Data = ex.Message,
                    Message = "Внутрення ошибка. Необработанное исключение"
                },
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                await context.Response.WriteAsync(res);

                return;
            }
        }
    }
}
