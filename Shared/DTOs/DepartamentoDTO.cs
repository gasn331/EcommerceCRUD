using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class DepartamentoDTO
    {
        public DepartamentoDTO()
        {
            Id = Guid.NewGuid();
            Codigo = string.Empty;
            Descricao = string.Empty;
        }

        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}
