using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Todo.BLL.Interfaces;
using Todo.BLL.Services;
using Todo.DAL.Entities;
using Todo.DAL.GenericRepository;
using Todo.DAL.UnitOfWork;
using TodoApiDTO.Logger;
using TodoApiDTO.Middleware;

namespace TodoApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private const string SwaggerTitle = "ToDoItem";
        private const string Version = "v1.0.0";
        private const string SwaggerEndpoint = "/swagger/v1.0.0/swagger.json";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddControllers();
            services.AddMvcCoreCustom();

            services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //рагистрация swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Version, new OpenApiInfo
                {
                    Title = SwaggerTitle,
                    Version = Version,
                    Contact = new OpenApiContact
                    {
                        Name = "Nurbek Akmatov",
                        Email = "akmatovn@gmail.com",
                    },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //регистрация сервисов
            services.AddTransient<IToDoItemService, ToDoItemService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Logging.LoggerFactory = loggerFactory;

            if (env.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
#if !DEBUG
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            var path = new OpenApiPaths();
                            var pathItem = new OpenApiPathItem();

                            path.Add(Configuration.GetValue<string>("Swagger:Context") + SwaggerEndpoint, pathItem);
                            swaggerDoc.Paths = path;
                        });
#endif
                });
            }

            app.UseSwaggerUI(c =>
            {
#if DEBUG
                c.SwaggerEndpoint(SwaggerEndpoint, SwaggerTitle);
#else
                c.SwaggerEndpoint(Configuration.GetValue<string>("Swagger:Context") + SwaggerEndpoint, SwaggerTitle);
#endif
                c.RoutePrefix = string.Empty;
            });

            app.UseMiddleware<HttpExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            //{
            //    var context = serviceScope?.ServiceProvider.GetRequiredService<TodoContext>();
            //    context.Database.Migrate();
            //}
        }
    }
}
