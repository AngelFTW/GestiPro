using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestiPro.API.Repositories;

public class TotaisRepository
{
    private readonly AppDbContext _context;

    public TotaisRepository(AppDbContext context)
    {
        _context = context;
    }


    // Retorna o total de receitas, despesas e saldo por pessoa 
    // e dos totais gerais de todas as pessoas. Pessoas sem transação aparecem com valor zero.
    public async Task<RelatorioTotaisPessoaDto> ObterTotaisPorPessoaAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
            .ToListAsync();

        var totaisPorPessoa = pessoas.Select(p => new TotaisPorPessoaDto
        {
            PessoaId = p.Id,
            PessoaNome = p.Nome,
            TotalReceitas = p.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor),
            TotalDespesas = p.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor)
        }).ToList();

        return new RelatorioTotaisPessoaDto
        {
            TotaisPorPessoa = totaisPorPessoa,
            TotalGeralReceitas = totaisPorPessoa.Sum(t => t.TotalReceitas),
            TotalGeralDespesas = totaisPorPessoa.Sum(t => t.TotalDespesas)
        };
    }

    // Retorna o total de receitas, despesas e saldo por categoria, além dos totais gerais consolidados 
    // de todas as categorias. Caso nao haja transações, categorias possuirão valor zero.
    public async Task<RelatorioTotaisCategoriaDto> ObterTotaisPorCategoriaAsync()
    {
        var categorias = await _context.Categorias
            .Include(c => c.Transacoes)
            .ToListAsync();

        var totaisPorCategoria = categorias.Select(c => new TotaisPorCategoriaDto
        {
            CategoriaId = c.Id,
            CategoriaDescricao = c.Descricao,
            TotalReceitas = c.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor),
            TotalDespesas = c.Transacoes
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor)
        }).ToList();

        return new RelatorioTotaisCategoriaDto
        {
            TotaisPorCategoria = totaisPorCategoria,
            TotalGeralReceitas = totaisPorCategoria.Sum(t => t.TotalReceitas),
            TotalGeralDespesas = totaisPorCategoria.Sum(t => t.TotalDespesas)
        };
    }
}
