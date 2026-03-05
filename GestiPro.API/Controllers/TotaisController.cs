using GestiPro.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestiPro.API.Controllers;

// Controller responsável pelas consultas de totais financeiros.
// Endpoints: GET /api/totais/por-pessoa
//            GET /api/totais/por-categoria
[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly TotaisRepository _totaisRepository;

    public TotaisController(TotaisRepository totaisRepository)
    {
        _totaisRepository = totaisRepository;
    }

    // Retorna o total de receitas, despesas etc por pessoa
    [HttpGet("por-pessoa")]
    public async Task<IActionResult> TotaisPorPessoa()
    {
        var relatorio = await _totaisRepository.ObterTotaisPorPessoaAsync();
        return Ok(relatorio);
    }

    // Retorna o total de receitas, despesas etc por categoria
    [HttpGet("por-categoria")]
    public async Task<IActionResult> TotaisPorCategoria()
    {
        var relatorio = await _totaisRepository.ObterTotaisPorCategoriaAsync();
        return Ok(relatorio);
    }
}
