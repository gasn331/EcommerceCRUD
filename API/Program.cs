using API.Data;
using API.MappingProfiles;
using API.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar servi�os ao cont�iner

            // Configura��o da string de conex�o MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Inje��o de depend�ncia de MySqlDataAccess
            builder.Services.AddSingleton(new MySqlDataAccess(connectionString));

            // Inje��o de depend�ncia dos servi�os
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Configura��o do AutoMapper com MappingProfile
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Configura��o de autentica��o removida

            // Adicionar controladores e suporte ao Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configurar o pipeline de requisi��o HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middleware de autentica��o e autoriza��o removido
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
