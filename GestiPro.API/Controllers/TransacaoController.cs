using GestiPro.API.DTOs;
using GestiPro.API.Models;
using GestiPro.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

// Controller responsável pelo cadastro de Transações.
// Endpoints: GET /api/transacoes, 
//            POST /api/transacoes
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoRepository _transacaoRepository;

    public TransacoesController(TransacaoRepository transacaoRepository)
    {
        _transacaoRepository = transacaoRepository;
    }

    // Lista todas as transações com dados de pessoa e categoria.
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var transacoes = await _transacaoRepository.ListarAsync();
        return Ok(transacoes);
    }

    // Cria uma nova transação com base nas regras de negócio:
    //    Menor de 18 anos não pode ter Receita.
    //    Categoria deve ser compatível com o tipo da transação.
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] TransacaoRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var resultado = await _transacaoRepository.CriarAsync(dto);

        if (!resultado.Sucesso)
            return BadRequest(new { mensagem = resultado.Erro });

        return CreatedAtAction(nameof(Listar), resultado.Dados);
    }
}
