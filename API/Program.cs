using API.Data;
using API.MappingProfiles;
using API.Services;
using AutoMapper;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar serviços ao contêiner

            // Configuração da string de conexão MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Injeção de dependência de MySqlDataAccess
            builder.Services.AddSingleton(new MySqlDataAccess(connectionString));

            // Injeção de dependência dos serviços
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();

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

            // Middleware de autenticação e autorização removidos para ignorar autenticação
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}