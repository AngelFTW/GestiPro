using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using GestiPro.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransacaoRepositoryTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // Cria pessoa e categoria no banco e retorna os IDs
    private async Task<(int idPessoa, int idCategoria)> SeedAsync(
        AppDbContext ctx,
        int idade = 25,
        CategoriaFinalidade finalidade = CategoriaFinalidade.Despesa)
    {
        var pessoaRepo = new PessoaRepository(ctx);
        var categoriaRepo = new CategoriaRepository(ctx);

        var pessoa = await pessoaRepo.CriarAsync(new PessoaRequestDto { Nome = "Pessoa teste", Idade = idade });
        var categoria = await categoriaRepo.CriarAsync(new CategoriaRequestDto { Descricao = "Categoria Teste", Finalidade = finalidade });

        return (pessoa.Id, categoria.Id);
    }

    [Fact]
    public async Task CriarAsync_DeveSalvarTransacao_QuandoDadosValidos()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Compra mercado",
            Valor = 150.00m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
        Assert.Equal("Compra mercado", resultado.Dados.Descricao);
        Assert.Equal(150.00m, resultado.Dados.Valor);
    }

    [Fact]
    public async Task CriarAsync_RetornaErro_QuandoPessoaNaoExiste()
    {
        var ctx = CriarContexto();
        var (_, idCategoria) = await SeedAsync(ctx);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = 999,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Pessoa não encontrada", resultado.Erro);
    }

    [Fact]
    public async Task CriarAsync_RetornaErro_QuandoCategoriaIncompativel_DespesaEmReceita()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx, finalidade: CategoriaFinalidade.Receita);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não é compatível", resultado.Erro);
    }

    [Fact]
    public async Task CriarAsync_RetornaErro_QuandoCategoriaIncompativel_ReceitaEmDespesa()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx, finalidade: CategoriaFinalidade.Despesa);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Receita,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não é compatível", resultado.Erro);
    }

    [Fact]
    public async Task CriarAsync_RetornaErro_QuandoMenorDeIdadeTentaReceita()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx, idade: 16, finalidade: CategoriaFinalidade.Receita);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Renda",
            Valor = 500m,
            Tipo = TipoTransacao.Receita,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Menores de idade", resultado.Erro);
    }

    [Fact]
    public async Task CriarAsync_Sucesso_QuandoMenorDeIdadeCriaDespesa()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx, idade: 16, finalidade: CategoriaFinalidade.Despesa);
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Lanche",
            Valor = 20m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
    }

    [Fact]
    public async Task CriarAsync_RetornaErro_QuandoCategoriaNaoExiste()
    {
        var ctx = CriarContexto();
        var pessoaRepo = new PessoaRepository(ctx);
        var pessoa = await pessoaRepo.CriarAsync(new PessoaRequestDto { Nome = "Teste", Idade = 25 });
        var repo = new TransacaoRepository(ctx);

        var dto = new TransacaoRequestDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = pessoa.Id,
            IdCategoria = 999
        };

        var resultado = await repo.CriarAsync(dto);

        Assert.False(resultado.Sucesso);
        Assert.Contains("Categoria não encontrada", resultado.Erro);
    }

    [Fact]
    public async Task CriarAsync_CategoriaAmbas_PermiteDespesaEReceita()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx, finalidade: CategoriaFinalidade.Ambas);
        var repo = new TransacaoRepository(ctx);

        var despesa = await repo.CriarAsync(new TransacaoRequestDto
        {
            Descricao = "Despesa geral",
            Valor = 50m,
            Tipo = TipoTransacao.Despesa,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        });

        var receita = await repo.CriarAsync(new TransacaoRequestDto
        {
            Descricao = "Receita geral",
            Valor = 200m,
            Tipo = TipoTransacao.Receita,
            IdPessoa = idPessoa,
            IdCategoria = idCategoria
        });

        Assert.True(despesa.Sucesso);
        Assert.True(receita.Sucesso);
    }

    [Fact]
    public async Task ListarAsync_RetornaTodasAsTransacoes()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx);
        var repo = new TransacaoRepository(ctx);

        await repo.CriarAsync(new TransacaoRequestDto { Descricao = "Transacao 1", Valor = 10m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });
        await repo.CriarAsync(new TransacaoRequestDto { Descricao = "Transacao 2", Valor = 20m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });

        var resultado = await repo.ListarAsync();

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task ListarAsync_RetornaListaVazia_QuandoNaoHaTransacoes()
    {
        var repo = new TransacaoRepository(CriarContexto());

        var resultado = await repo.ListarAsync();

        Assert.Empty(resultado);
    }
}
