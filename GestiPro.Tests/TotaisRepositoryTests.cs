using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Models;
using GestiPro.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class TotaisRepositoryTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // Cria pessoa, categoria e transação diretamente no contexto para montar cenários de teste
    private async Task<(int idPessoa, int idCategoria)> SeedAsync(
        AppDbContext ctx,
        string nomePessoa = "Pessoa teste",
        int idade = 30,
        CategoriaFinalidade finalidade = CategoriaFinalidade.Ambas)
    {
        var pessoaRepo = new PessoaRepository(ctx);
        var categoriaRepo = new CategoriaRepository(ctx);

        var pessoa = await pessoaRepo.CriarAsync(new PessoaRequestDto { Nome = nomePessoa, Idade = idade });
        var categoria = await categoriaRepo.CriarAsync(new CategoriaRequestDto { Descricao = "Categoria teste", Finalidade = finalidade });

        return (pessoa.Id, categoria.Id);
    }

    [Fact]
    public async Task ObterTotaisPorPessoaAsync_RetornaListaVazia_QuandoNaoHaPessoas()
    {
        var repo = new TotaisRepository(CriarContexto());

        var resultado = await repo.ObterTotaisPorPessoaAsync();

        Assert.Empty(resultado.TotaisPorPessoa);
        Assert.Equal(0, resultado.TotalGeralReceitas);
        Assert.Equal(0, resultado.TotalGeralDespesas);
        Assert.Equal(0, resultado.SaldoLiquido);
    }

    [Fact]
    public async Task ObterTotaisPorPessoaAsync_PessoaSemTransacoes_RetornaZerado()
    {
        var ctx = CriarContexto();
        var pessoaRepo = new PessoaRepository(ctx);
        await pessoaRepo.CriarAsync(new PessoaRequestDto { Nome = "Sem Transações", Idade = 25 });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorPessoaAsync();

        Assert.Single(resultado.TotaisPorPessoa);
        Assert.Equal(0, resultado.TotaisPorPessoa[0].TotalReceitas);
        Assert.Equal(0, resultado.TotaisPorPessoa[0].TotalDespesas);
        Assert.Equal(0, resultado.TotaisPorPessoa[0].Saldo);
    }

    [Fact]
    public async Task ObterTotaisPorPessoaAsync_CalculaReceitasEDespesasCorretamente()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx);
        var transacaoRepo = new TransacaoRepository(ctx);

        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Salário", Valor = 3000m, Tipo = TipoTransacao.Receita, IdPessoa = idPessoa, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Aluguel", Valor = 1200m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Mercado", Valor = 500m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorPessoaAsync();

        var pessoa = resultado.TotaisPorPessoa[0];
        Assert.Equal(3000m, pessoa.TotalReceitas);
        Assert.Equal(1700m, pessoa.TotalDespesas);
        Assert.Equal(1300m, pessoa.Saldo);
    }

    [Fact]
    public async Task ObterTotaisPorPessoaAsync_TotaisGeraisConsolidamTodasAsPessoas()
    {
        var ctx = CriarContexto();
        var (idPessoa1, idCategoria) = await SeedAsync(ctx, "Pessoa 1");
        var pessoaRepo = new PessoaRepository(ctx);
        var pessoa2 = await pessoaRepo.CriarAsync(new PessoaRequestDto { Nome = "Pessoa 2", Idade = 40 });
        var transacaoRepo = new TransacaoRepository(ctx);

        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Receita P1", Valor = 1000m, Tipo = TipoTransacao.Receita, IdPessoa = idPessoa1, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Receita P2", Valor = 2000m, Tipo = TipoTransacao.Receita, IdPessoa = pessoa2.Id, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Despesa P2", Valor = 500m, Tipo = TipoTransacao.Despesa, IdPessoa = pessoa2.Id, IdCategoria = idCategoria });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorPessoaAsync();

        Assert.Equal(2, resultado.TotaisPorPessoa.Count);
        Assert.Equal(3000m, resultado.TotalGeralReceitas);
        Assert.Equal(500m, resultado.TotalGeralDespesas);
        Assert.Equal(2500m, resultado.SaldoLiquido);
    }

    [Fact]
    public async Task ObterTotaisPorPessoaAsync_SaldoNegativo_QuandoDespesasMaiorQueReceitas()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx);
        var transacaoRepo = new TransacaoRepository(ctx);

        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Receita", Valor = 100m, Tipo = TipoTransacao.Receita, IdPessoa = idPessoa, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Despesa", Valor = 500m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorPessoaAsync();

        Assert.Equal(-400m, resultado.TotaisPorPessoa[0].Saldo);
        Assert.Equal(-400m, resultado.SaldoLiquido);
    }

    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_RetornaListaVazia_QuandoNaoHaCategorias()
    {
        var repo = new TotaisRepository(CriarContexto());

        var resultado = await repo.ObterTotaisPorCategoriaAsync();

        Assert.Empty(resultado.TotaisPorCategoria);
        Assert.Equal(0, resultado.TotalGeralReceitas);
        Assert.Equal(0, resultado.TotalGeralDespesas);
        Assert.Equal(0, resultado.SaldoLiquido);
    }

    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_CategoriaSemTransacoes_RetornaZerado()
    {
        var ctx = CriarContexto();
        var categoriaRepo = new CategoriaRepository(ctx);
        await categoriaRepo.CriarAsync(new CategoriaRequestDto { Descricao = "Sem uso", Finalidade = CategoriaFinalidade.Ambas });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorCategoriaAsync();

        Assert.Single(resultado.TotaisPorCategoria);
        Assert.Equal(0, resultado.TotaisPorCategoria[0].TotalReceitas);
        Assert.Equal(0, resultado.TotaisPorCategoria[0].TotalDespesas);
    }

    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_CalculaTotaisCorretamente()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCategoria) = await SeedAsync(ctx);
        var transacaoRepo = new TransacaoRepository(ctx);

        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Receita", Valor = 2000m, Tipo = TipoTransacao.Receita, IdPessoa = idPessoa, IdCategoria = idCategoria });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Despesa", Valor = 800m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = idCategoria });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorCategoriaAsync();

        var categoria = resultado.TotaisPorCategoria[0];
        Assert.Equal(2000m, categoria.TotalReceitas);
        Assert.Equal(800m, categoria.TotalDespesas);
        Assert.Equal(1200m, categoria.Saldo);
    }

    [Fact]
    public async Task ObterTotaisPorCategoriaAsync_TotaisGeraisConsolidamTodasAsCategorias()
    {
        var ctx = CriarContexto();
        var (idPessoa, idCat1) = await SeedAsync(ctx);
        var categoriaRepo = new CategoriaRepository(ctx);
        var cat2 = await categoriaRepo.CriarAsync(new CategoriaRequestDto { Descricao = "Categoria 2", Finalidade = CategoriaFinalidade.Ambas });
        var transacaoRepo = new TransacaoRepository(ctx);

        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Receita Categoria 1", Valor = 1000m, Tipo = TipoTransacao.Receita, IdPessoa = idPessoa, IdCategoria = idCat1 });
        await transacaoRepo.CriarAsync(new TransacaoRequestDto { Descricao = "Despesa Categoria 2", Valor = 300m, Tipo = TipoTransacao.Despesa, IdPessoa = idPessoa, IdCategoria = cat2.Id });

        var repo = new TotaisRepository(ctx);
        var resultado = await repo.ObterTotaisPorCategoriaAsync();

        Assert.Equal(2, resultado.TotaisPorCategoria.Count);
        Assert.Equal(1000m, resultado.TotalGeralReceitas);
        Assert.Equal(300m, resultado.TotalGeralDespesas);
        Assert.Equal(700m, resultado.SaldoLiquido);
    }
}
