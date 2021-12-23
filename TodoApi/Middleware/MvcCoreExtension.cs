using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TodoApiDTO.Models;

namespace TodoApiDTO.Middleware
{
    public static class MvcCoreExtension
    {
        public static IMvcCoreBuilder AddMvcCoreCustom(this IServiceCollection builder)
        {
            return builder.AddMvcCore()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .Select(e => new
                            {
                                Name = e.Key,
                                Message = e.Value.Errors.First().ErrorMessage
                            }).ToArray();
                        return new BadRequestObjectResult(new BaseResponse<dynamic>
                        {
                            Code = -1,
                            Message = "Неверная модель!",
                            Data = errors
                        });
                    };
                })
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddApiExplorer();
        }
    }
}
