using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC.Services;
using MVC.Services.Interfaces;
using System.Net.Http.Headers;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar servi�os ao cont�iner
            builder.Services.AddControllersWithViews();

            // Configurar HttpClient
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Adicionar outros servi�os necess�rios
            builder.Services.AddTransient<IProdutoService, ProdutoService>();
            builder.Services.AddTransient<IDepartamentoService, DepartamentoService>();
            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            var app = builder.Build();

            // Configurar o pipeline de requisi��es HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            // Middleware de autentica��o e autoriza��o removido

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
