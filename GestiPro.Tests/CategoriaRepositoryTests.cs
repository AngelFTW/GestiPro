using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using GestiPro.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class CategoriaRepositoryTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>() 
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options); // Cria um banco novo para cada teste
    }

    [Fact]
    public async Task CriarAsync_DeveSalvarCategoria()
    {
        var repo = new CategoriaRepository(CriarContexto());
        var dto = new CategoriaRequestDto { Descricao = "Alimentação", Finalidade = CategoriaFinalidade.Despesa };

        var resultado = await repo.CriarAsync(dto);

        Assert.Equal("Alimentação", resultado.Descricao);
        Assert.Equal(CategoriaFinalidade.Despesa, resultado.Finalidade);
        Assert.True(resultado.Id > 0);
    }

    [Fact]
    public async Task ListarAsync_RetornaListaVazia_QuandoNaoHaCategorias()
    {
        var repo = new CategoriaRepository(CriarContexto());

        var resultado = await repo.ListarAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ListarAsync_RetornaTodasAsCategorias()
    {
        var ctx = CriarContexto();
        var repo = new CategoriaRepository(ctx);
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Salário", Finalidade = CategoriaFinalidade.Receita });
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Aluguel", Finalidade = CategoriaFinalidade.Despesa });

        var resultado = await repo.ListarAsync();

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task ListarPorTipoTransacaoAsync_RetornaApenasCategoriasDespesa_QuandoTipoDespesa()
    {
        var ctx = CriarContexto();
        var repo = new CategoriaRepository(ctx);
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Aluguel", Finalidade = CategoriaFinalidade.Despesa });
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Salário", Finalidade = CategoriaFinalidade.Receita });

        var resultado = await repo.ListarPorTipoTransacaoAsync(TipoTransacao.Despesa);

        Assert.Single(resultado);
        Assert.Equal("Aluguel", resultado[0].Descricao);
    }

    [Fact]
    public async Task ListarPorTipoTransacaoAsync_RetornaApenasCategoriasReceita_QuandoTipoReceita()
    {
        var ctx = CriarContexto();
        var repo = new CategoriaRepository(ctx);
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Aluguel", Finalidade = CategoriaFinalidade.Despesa });
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Salário", Finalidade = CategoriaFinalidade.Receita });

        var resultado = await repo.ListarPorTipoTransacaoAsync(TipoTransacao.Receita);

        Assert.Single(resultado);
        Assert.Equal("Salário", resultado[0].Descricao);
    }

    [Fact]
    public async Task ListarPorTipoTransacaoAsync_IncluiCategoriaAmbas_QuandoTipoDespesa()
    {
        var ctx = CriarContexto();
        var repo = new CategoriaRepository(ctx);
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Geral", Finalidade = CategoriaFinalidade.Ambas });
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Salário", Finalidade = CategoriaFinalidade.Receita });

        var resultado = await repo.ListarPorTipoTransacaoAsync(TipoTransacao.Despesa);

        Assert.Single(resultado);
        Assert.Equal("Geral", resultado[0].Descricao);
    }

    [Fact]
    public async Task ListarPorTipoTransacaoAsync_IncluiCategoriaAmbas_QuandoTipoReceita()
    {
        var ctx = CriarContexto();
        var repo = new CategoriaRepository(ctx);
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Geral", Finalidade = CategoriaFinalidade.Ambas });
        await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Aluguel", Finalidade = CategoriaFinalidade.Despesa });

        var resultado = await repo.ListarPorTipoTransacaoAsync(TipoTransacao.Receita);

        Assert.Single(resultado);
        Assert.Equal("Geral", resultado[0].Descricao);
    }

    [Fact]
    public async Task FinalidadeDescricao_DeveRetornarTextoCorreto()
    {
        var repo = new CategoriaRepository(CriarContexto());
        var despesa = await repo.CriarAsync(new CategoriaRequestDto { Descricao = "X", Finalidade = CategoriaFinalidade.Despesa });
        var receita = await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Y", Finalidade = CategoriaFinalidade.Receita });
        var ambas = await repo.CriarAsync(new CategoriaRequestDto { Descricao = "Z", Finalidade = CategoriaFinalidade.Ambas });

        Assert.Equal("Despesa", despesa.FinalidadeDescricao);
        Assert.Equal("Receita", receita.FinalidadeDescricao);
        Assert.Equal("Ambas", ambas.FinalidadeDescricao);
    }
}
