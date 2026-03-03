namespace GestiPro.API.Models;

public class Transacao
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public int IdPessoa { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Categoria Categoria { get; set; }
    public Pessoa Pessoa { get; set; }
}

public enum TipoTransacao
{
    Despesa = 0,
    Receita = 1
}
