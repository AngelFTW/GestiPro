using GestiPro.API.DTOs;
using GestiPro.API.Models;
using GestiPro.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

// Controller responsável pelo cadastro de Categorias.
// Endpoints: GET /api/categorias, 
//            POST /api/categorias, 
//            GET /api/categorias/por-tipo/{tipo}
[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaRepository _categoriaRepository;

    public CategoriasController(CategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    // Lista todas as categorias cadastradas.
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _categoriaRepository.ListarAsync();
        return Ok(categorias);
    }

    // Lista categorias compatíveis com um tipo de transação (Despesa=0, Receita=1).
    [HttpGet("por-tipo/{tipo}")]
    public async Task<IActionResult> ListarPorTipo(TipoTransacao tipo)
    {
        var categorias = await _categoriaRepository.ListarPorTipoTransacaoAsync(tipo);
        return Ok(categorias);
    }

    // Cria uma nova categoria.
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CategoriaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var criada = await _categoriaRepository.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), criada);
    }
}
