using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAlunos.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrudAlunos
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {

            //Configurar contexto com o SQLServer
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost;Database=MeusContatos;Trusted_Connection=True;TrustServerCertificate=True"));

            //Adicionar suporte para controladores
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Versao1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Minha API",
                    Version = "Versao1"
                });
            });


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Versao1/swagger.json", "Minha API Versao1");
                c.RoutePrefix = string.Empty; // Deixa a documentacao no root localhost:5000
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
