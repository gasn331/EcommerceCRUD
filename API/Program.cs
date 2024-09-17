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

            // Configuração da string de conexão MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Adicionar serviços ao contêiner
            builder.Services.AddScoped<IMySqlDataAccess>(provider => new MySqlDataAccess(connectionString));

            // Injeção de dependência dos serviços
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Configuração do AutoMapper com MappingProfile
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Adicionar controladores e suporte ao Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configurar o pipeline de requisição HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middleware de autenticação e autorização removido
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
