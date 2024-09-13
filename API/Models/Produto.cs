namespace API.Models
{
    public class Produto
    {
        public Produto() 
        {
            Id = Guid.NewGuid();
            Departamento = new Departamento();
            DepartamentoId = Departamento.Id;
            Codigo = string.Empty;
            Descricao = string.Empty;
            Excluido = false;
        }

        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }

        //Departamento
        public Guid DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        public decimal Preco { get; set; }
        public bool Status {  get; set; }
    }
}
