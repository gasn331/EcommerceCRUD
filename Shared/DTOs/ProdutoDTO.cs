using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProdutoDTO
    {
        public ProdutoDTO() 
        {
            Id = Guid.NewGuid();
            Codigo = string.Empty;
            Descricao = string.Empty;
            Departamento = new DepartamentoDTO();
        }

        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public DepartamentoDTO Departamento { get; set; }
        public bool Excluido { get; set; }
        public decimal Preco { get; set; }
        public bool Status { get; set; }
    }
}
