
using API.Data;
using AutoMapper;
using Shared.DTOs;

namespace API.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IMySqlDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public DepartamentoService(IMySqlDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartamentoDTO>> GetAllDepartamentosAsync()
        {
            var departamentos = await _dataAccess.GetDepartamentosAsync();

            if (departamentos != null && departamentos.ToList() != null)
                return _mapper.Map<IEnumerable<DepartamentoDTO>>(departamentos);

            return new List<DepartamentoDTO>();
        }

        public async Task<DepartamentoDTO> GetDepartamentoAsync(string codigo)
        {
            var departamento = await _dataAccess.GetDepartamentoAsync(codigo);

            if(departamento != null)
                return _mapper.Map<DepartamentoDTO>(departamento);

            return new DepartamentoDTO();
        }
    }
}
