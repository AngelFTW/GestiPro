using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestiPro.API.Models;

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
