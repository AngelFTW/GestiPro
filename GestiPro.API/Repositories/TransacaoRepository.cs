using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestiPro.API.Repositories;

public record TransacaoResult(bool Sucesso, string Erro = null, TransacaoResponseDto Dados = null);
public class TransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Retorna todas as transações com dados de pessoa e categoria.</summary>
    public async Task<List<TransacaoResponseDto>> ListarAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .Select(t => new TransacaoResponseDto
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Tipo = t.Tipo,
                IdCategoria = t.IdCategoria,
                CategoriaDescricao = t.Categoria.Descricao,
                IdPessoa = t.IdPessoa,
                PessoaNome = t.Pessoa.Nome
            })
            .ToListAsync();
    }

    // Cria nova transacao respeitando as regras de negocio apresentadas
    public async Task<TransacaoResult> CriarAsync(TransacaoRequestDto dto)
    {
        // Busca a pessoa vinculada
        var pessoa = await _context.Pessoas.FindAsync(dto.IdPessoa);
        if (pessoa == null)
            return new TransacaoResult(false, "Pessoa não encontrada.");

        // Regra: menores de 18 anos só podem ter despesas
        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
            return new TransacaoResult(false, "Menores de idade só podem registrar transações do tipo Despesa.");

        // Busca a categoria vinculada
        var categoria = await _context.Categorias.FindAsync(dto.IdCategoria);
        if (categoria == null)
            return new TransacaoResult(false, "Categoria não encontrada.");

        // Regra: categoria deve ser compatível com o tipo da transação
        var categoriaIncompativel =
            (dto.Tipo == TipoTransacao.Despesa && categoria.Finalidade == CategoriaFinalidade.Receita) ||
            (dto.Tipo == TipoTransacao.Receita && categoria.Finalidade == CategoriaFinalidade.Despesa);

        if (categoriaIncompativel)
            return new TransacaoResult(false, $"A categoria '{categoria.Descricao}' não é compatível com transações do tipo '{dto.Tipo}'.");

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            IdCategoria = dto.IdCategoria,
            IdPessoa = dto.IdPessoa
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return new TransacaoResult(true, Dados: new TransacaoResponseDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            IdCategoria = transacao.IdCategoria,
            CategoriaDescricao = categoria.Descricao,
            IdPessoa = transacao.IdPessoa,
            PessoaNome = pessoa.Nome
        });
    }
}
