using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksApp.Data;
using TasksApp.Services;

namespace TasksApp
{
    // Startup.cs
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<TaskContext>(options => {
                options.UseInMemoryDatabase("TasksDb");
                var jsonFilePath = Configuration["TaskData:JsonFilePath"];
            });

            services.AddSingleton<LogService>();  

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44369;
            });

            // Optional: add Swagger for testing
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TaskContext db)
        {
            db.LoadDataFromJson();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Optional: Swagger
                app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                //                    c.RoutePrefix = "swagger";);
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API v1");
                    c.RoutePrefix = "swagger";
                    c.SupportedSubmitMethods(new[] {
                        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get,
                        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post,
                        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put,
                        Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete
                    });
                });

            }

            app.UseHttpsRedirection(); // 🔐 This enforces HTTPS

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
