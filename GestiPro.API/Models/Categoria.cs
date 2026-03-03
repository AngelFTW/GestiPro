namespace GestiPro.API.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public CategoriaFinalidade Finalidade { get; set; }
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}

public enum CategoriaFinalidade
{
    Despesa = 0,
    Receita = 1,
    Ambas = 2
}
