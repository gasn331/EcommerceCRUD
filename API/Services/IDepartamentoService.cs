using Shared.DTOs;

namespace API.Services
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<DepartamentoDTO>> GetAllDepartamentosAsync();
        Task<DepartamentoDTO> GetDepartamentoAsync(string codigo);
    }
}
