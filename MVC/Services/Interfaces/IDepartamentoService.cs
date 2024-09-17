using Shared.DTOs;

namespace MVC.Services.Interfaces
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<DepartamentoDTO>> GetDepartamentosAsync();
        Task<DepartamentoDTO> GetDepartamentoAsync(string codigo);
    }
}