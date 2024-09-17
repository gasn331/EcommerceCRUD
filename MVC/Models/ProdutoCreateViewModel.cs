using Shared.DTOs;
using System.Collections.Generic;

public class ProdutoCreateViewModel
{
    public ProdutoDTO Produto { get; set; }
    public IEnumerable<DepartamentoDTO> DepartamentosDisponiveis { get; set; }
    public string DepartamentoSelecionadoCodigo { get; set; }  // ID do departamento selecionado
}
