namespace API.Models
{
    public class Departamento
    {
        public Departamento() 
        {
            Id = Guid.NewGuid();
            Codigo = string.Empty;
            Descricao = string.Empty;
            Produtos = new List<Produto>();
        }

        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public IEnumerable<Produto> Produtos { get; set; }
    }
}
