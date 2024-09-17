using API.Models;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Shared.DTOs;
using System.Data;

namespace API.Data
{
    public class MySqlDataAccess
    {
        #region Definições
        private readonly string _connectionString;

        public MySqlDataAccess(string connectionString) 
        {
            _connectionString = connectionString;
        }
       
        private MySqlConnection GetConnection() 
        {
            return new MySqlConnection(_connectionString);
        }
        #endregion

        #region Métodos para Produto
        public async Task<IEnumerable<Produto>> GetProdutosAsync(int pageNumber, int pageSize) 
        {
            var produtos = new List<Produto>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(@"SELECT P.*, D.Codigo AS DepartamentoCodigo, D.Descricao AS DepartamentoDescricao  
                                                 FROM Produtos P INNER JOIN Departamentos D ON P.DepartamentoId = D.Id 
                                                 WHERE P.Excluido = FALSE
                                                 ORDER BY Codigo
                                                 LIMIT @Offset, @PageSize", connection);

                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = await command.ExecuteReaderAsync()) 
                {
                    while(await reader.ReadAsync())
                    {
                        produtos.Add(new Produto
                        {
                            Id = reader.GetGuid("Id"),
                            Codigo = reader.GetString("Codigo"),
                            Descricao = reader.GetString("Descricao"),
                            DepartamentoId = reader.GetGuid("DepartamentoId"),
                            Departamento = new Departamento 
                            { 
                                Id = reader.GetGuid("DepartamentoId"),
                                Codigo = reader.GetString("DepartamentoCodigo"),
                                Descricao = reader.GetString("DepartamentoDescricao"),
                            },
                            Excluido = reader.GetBoolean("Excluido"),
                            Preco = reader.GetDecimal("Preco"),
                            Status = reader.GetBoolean("Status")
                        });
                    }
                }
            }

            return produtos;
        }

        public async Task<Produto> GetProdutoAsync(string codigo) 
        {
            var produto = new Produto();

            using (var connection = GetConnection()) 
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(@"SELECT P.*, D.Codigo AS DepartamentoCodigo, D.Descricao AS DepartamentoDescricao  
                                                 FROM Produtos P INNER JOIN Departamentos D ON P.DepartamentoId = D.Id
                                                 WHERE P.Codigo = @Codigo AND P.Excluido = FALSE", connection);

                command.Parameters.AddWithValue("@Codigo", codigo);

                using (var reader = await command.ExecuteReaderAsync()) 
                {
                    while (await reader.ReadAsync()) 
                    {
                        produto.Id = reader.GetGuid("Id");
                        produto.Codigo = reader.GetString("Codigo");
                        produto.Descricao = reader.GetString("Descricao");
                        produto.DepartamentoId = reader.GetGuid("DepartamentoId");
                        produto.Departamento = new Departamento
                        {
                            Id = reader.GetGuid("DepartamentoId"),
                            Codigo = reader.GetString("DepartamentoCodigo"),
                            Descricao = reader.GetString("DepartamentoDescricao")
                        };
                        produto.Excluido = reader.GetBoolean("Excluido");
                        produto.Preco = reader.GetDecimal("Preco");
                        produto.Status = reader.GetBoolean("Status");
                    }
                }
            }

            return produto;
        }

        public async Task<string> CreateProdutoAsync(Produto produto) 
        {
            using (var connection = GetConnection()) 
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(@"INSERT INTO Produtos (Codigo, Descricao, DepartamentoId, Preco, Status, Excluido) 
                                                 VALUES (@Codigo, @Descricao, @DepartamentoId, @Preco, @Status, FALSE); 
                                                 SELECT Id FROM Produtos WHERE Codigo = @Codigo; ", connection);

                command.Parameters.AddWithValue("@Codigo", produto.Codigo);
                command.Parameters.AddWithValue("@Descricao", produto.Descricao);
                command.Parameters.AddWithValue("@DepartamentoId", produto.DepartamentoId);
                command.Parameters.AddWithValue("@Preco", produto.Preco);
                command.Parameters.AddWithValue("@Status", produto.Status);

                var id = Convert.ToString(await command.ExecuteScalarAsync());

                return id;
            }
        }

        public async Task<bool> DeleteProdutoAsync(string codigo) 
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("UPDATE Produtos SET Excluido = TRUE WHERE Codigo = @Codigo", connection);

                command.Parameters.AddWithValue("@Codigo", codigo);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateProdutoAsync(Produto produto) 
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand(@"UPDATE Produtos SET Codigo = @Codigo, Descricao = @Descricao, DepartamentoId = @DepartamentoId, Excluido = @Excluido, Preco = @Preco, Status = @Status 
                                                 WHERE Codigo = @Codigo", connection);

                command.Parameters.AddWithValue("@Codigo", produto.Codigo);
                command.Parameters.AddWithValue("@Descricao", produto.Descricao);
                command.Parameters.AddWithValue("@DepartamentoId", produto.DepartamentoId);
                command.Parameters.AddWithValue("@Excluido", produto.Excluido);
                command.Parameters.AddWithValue("@Preco", produto.Preco);
                command.Parameters.AddWithValue("@Status", produto.Status);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        #endregion

        #region Métodos para Departamento
        public async Task<IEnumerable<Departamento>> GetDepartamentosAsync()
        {
            var departamentos = new List<Departamento>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("SELECT * FROM Departamentos", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        departamentos.Add(new Departamento
                        {
                            Id = reader.GetGuid("Id"),
                            Codigo = reader.GetString("Codigo"),
                            Descricao = reader.GetString("Descricao"),
                        });
                    }
                }
            }

            return departamentos;
        }

        public async Task<Departamento> GetDepartamentoAsync(string codigo)
        {
            var departamento = new Departamento();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("SELECT * FROM Departamentos WHERE Codigo = @Codigo", connection);
                command.Parameters.AddWithValue("@Codigo", codigo);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        departamento.Id = reader.GetGuid("Id");
                        departamento.Codigo = reader.GetString("Codigo");
                        departamento.Descricao = reader.GetString("Descricao");
                    }
                }
            }

            return departamento;
        }

        public async Task<int> GetTotalCountAsync()
        {
            int totalCount = 0;
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("SELECT COUNT(*) AS TotalCount FROM Produtos", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        totalCount = reader.GetInt32("TotalCount");
                    }
                }
            }

            return totalCount;
        }
        #endregion

        #region Métodos para User
        public async Task<bool> CreateUserAsync(string email, string passwordHash)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("INSERT INTO Autenticacao (Email, PasswordHash) VALUES (@Email, @PasswordHash)", connection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<string> GetPasswordHash(string email)
        {
            var passwordHash = string.Empty;

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var command = new MySqlCommand("SELECT PasswordHash FROM Autenticacao WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        passwordHash = reader.GetString("PasswordHash");
                    }
                }
            }

            return passwordHash;
        }
        #endregion

    }
}
