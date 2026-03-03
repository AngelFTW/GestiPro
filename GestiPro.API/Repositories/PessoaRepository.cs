using GestiPro.API.Data;
using GestiPro.API.Models;
using GestiPro.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GestiPro.API.Repositories;

public class PessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    // Lista todas as pesssoas cadastradas
    public async Task<List<PessoaResponseDto>> ListarAsync()
    {
        return await _context.Pessoas
            .Select(p => new PessoaResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade
            })
            .ToListAsync();
    }

    // Obter Pessoa por ID
    public async Task<PessoaResponseDto> ObterPorIdAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null) 
            return null;

        return new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        };
    }

    // Criar Pessoa Nova
    public async Task<PessoaResponseDto> CriarAsync(PessoaRequestDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };
        
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        };
    }

    //Atualizar Pessoa
    public async Task<bool> AtualizarAsync(int id, PessoaRequestDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null) 
            return false;

        pessoa.Nome = dto.Nome;
        pessoa.Idade = dto.Idade;
        await _context.SaveChangesAsync();
        
        return true;
    }

    //Deletar Pessoa
    public async Task<bool> DeletarAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null) 
            return false;

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
