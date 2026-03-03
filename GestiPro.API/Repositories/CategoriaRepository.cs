using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestiPro.API.Repositories;

public class CategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    // Listar todas as categorias
    public async Task<List<CategoriaResponseDto>> ListarAsync()
    {
        return await _context.Categorias
            .Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade
            })
            .ToListAsync();
    }

    //Criar Categoria
    public async Task<CategoriaResponseDto> CriarAsync(CategoriaRequestDto dto)
    {
        var categoria = new Categoria
        {
            Descricao = dto.Descricao,
            Finalidade = dto.Finalidade
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return new CategoriaResponseDto
        {
            Id = categoria.Id,
            Descricao = categoria.Descricao,
            Finalidade = categoria.Finalidade
        };
    }


    // Retorna categorias por tipo de transação
    //      se o tipo for Despesa, retorna categorias com finalidade Despesa ou Ambas.
    //      se o tipo for Receita, retorna categorias com finalidade Receita ou Ambas.
    public async Task<List<CategoriaResponseDto>> ListarPorTipoTransacaoAsync(TipoTransacao tipo)
    {
        var query = _context.Categorias.AsQueryable();

        query = tipo == TipoTransacao.Despesa
            ? query.Where(c => c.Finalidade == CategoriaFinalidade.Despesa || c.Finalidade == CategoriaFinalidade.Ambas)
            : query.Where(c => c.Finalidade == CategoriaFinalidade.Receita || c.Finalidade == CategoriaFinalidade.Ambas);

        return await query.Select(c => new CategoriaResponseDto
        {
            Id = c.Id,
            Descricao = c.Descricao,
            Finalidade = c.Finalidade
        }).ToListAsync();
    }
}
