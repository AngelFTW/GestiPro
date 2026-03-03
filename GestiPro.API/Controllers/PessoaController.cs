using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestiPro.API.DTOs;
using GestiPro.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestiPro.API.Controllers;

// Controller responsável pelo CRUD de Pessoas.
// Endpoints: GET /api/pessoas, 
//            POST /api/pessoas, 
//            PUT /api/pessoas/{id}, 
//            DELETE /api/pessoas/{id}
[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
private readonly PessoaRepository _pessoaRepository;

    public PessoaController(PessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    // Lista todas as pessoas cadastradas.
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pessoas = await _pessoaRepository.ListarAsync();
        return Ok(pessoas);
    }

    // Busca uma pessoa pelo ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        if (pessoa == null) return NotFound(new { mensagem = "Pessoa não encontrada." });
        return Ok(pessoa);
    }

    // Cria uma nova pessoa.
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PessoaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var criada = await _pessoaRepository.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    // Atualiza os dados de uma pessoa existente.
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] PessoaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var atualizada = await _pessoaRepository.AtualizarAsync(id, dto);
        if (!atualizada)
            return NotFound(new { mensagem = "Pessoa não encontrada." });
            
        return NoContent();
    }

    // Deleta uma pessoa e suas transações.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var deletada = await _pessoaRepository.DeletarAsync(id);
        if (!deletada)
            return NotFound(new { mensagem = "Pessoa não encontrada." });

        return NoContent();
    }
}
