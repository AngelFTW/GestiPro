using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestiPro.API.DTOs;

public class TotaisPorPessoaDto
{
    public int PessoaId { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class TotaisPorCategoriaDto
{
    public int CategoriaId { get; set; }
    public string CategoriaDescricao { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public class RelatorioTotaisPessoaDto
{
    public List<TotaisPorPessoaDto> TotaisPorPessoa { get; set; } = new();
    public decimal TotalGeralReceitas { get; set; }
    public decimal TotalGeralDespesas { get; set; }
    public decimal SaldoLiquido => TotalGeralReceitas - TotalGeralDespesas;
}

public class RelatorioTotaisCategoriaDto
{
    public List<TotaisPorCategoriaDto> TotaisPorCategoria { get; set; } = new();
    public decimal TotalGeralReceitas { get; set; }
    public decimal TotalGeralDespesas { get; set; }
    public decimal SaldoLiquido => TotalGeralReceitas - TotalGeralDespesas;
}
