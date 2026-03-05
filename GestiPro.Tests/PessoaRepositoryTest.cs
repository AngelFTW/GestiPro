using GestiPro.API.Data;
using GestiPro.API.DTOs;
using GestiPro.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class PessoaRepositoryTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // cria um banco novo pra cada teste
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CriarAsync_DeveSalvarPessoa()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);

        var dto = new PessoaRequestDto {
            Nome = "João",
            Idade = 27
        };
        var resultado = await repo.CriarAsync(dto);

        Assert.Equal("João", resultado.Nome);
        Assert.Equal(27, resultado.Idade);
        Assert.True(resultado.Id > 0);
    }

    [Fact]
    public async Task ObterPorIdAsync_RetornaNull_QuandoNaoExiste()
    {
        var repo = new PessoaRepository(CriarContexto());

        var resultado = await repo.ObterPorIdAsync(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task DeletarAsync_RetornaFalse_QuandoNaoExiste()
    {
        var repo = new PessoaRepository(CriarContexto());

        var resultado = await repo.DeletarAsync(999);

        Assert.False(resultado);
    }

    [Fact]
    public async Task ListarAsync_RetornaListaVazia_QuandoNaoHaPessoas()
    {
        var repo = new PessoaRepository(CriarContexto());

        var resultado = await repo.ListarAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ListarAsync_RetornaTodasAsPessoas()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);
        await repo.CriarAsync(new PessoaRequestDto { Nome = "Ana", Idade = 30 });
        await repo.CriarAsync(new PessoaRequestDto { Nome = "Carlos", Idade = 22 });

        var resultado = await repo.ListarAsync();

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task AtualizarAsync_RetornaFalse_QuandoNaoExiste()
    {
        var repo = new PessoaRepository(CriarContexto());

        var resultado = await repo.AtualizarAsync(999, new PessoaRequestDto { Nome = "X", Idade = 20 });

        Assert.False(resultado);
    }

    [Fact]
    public async Task AtualizarAsync_AtualizaDados_QuandoExiste()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);
        var criada = await repo.CriarAsync(new PessoaRequestDto { Nome = "Anna", Idade = 20 });

        var resultado = await repo.AtualizarAsync(criada.Id, new PessoaRequestDto { Nome = "Ana", Idade = 25 });
        var atualizada = await repo.ObterPorIdAsync(criada.Id);

        Assert.True(resultado);
        Assert.Equal("Ana", atualizada.Nome);
        Assert.Equal(25, atualizada.Idade);
    }

    [Fact]
    public async Task DeletarAsync_RetornaTrue_QuandoExiste()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);
        var criada = await repo.CriarAsync(new PessoaRequestDto { Nome = "Maria Deletada", Idade = 30 });

        var resultado = await repo.DeletarAsync(criada.Id);
        var aposDelecao = await repo.ObterPorIdAsync(criada.Id);

        Assert.True(resultado);
        Assert.Null(aposDelecao);
    }

    [Fact]
    public async Task MenorIdade_DeveSerTrue_QuandoIdadeMenorQue18()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);
        var criada = await repo.CriarAsync(new PessoaRequestDto { Nome = "Ana Dmenor", Idade = 15 });

        Assert.True(criada.MenorIdade);
    }

    [Fact]
    public async Task MenorIdade_DeveSerFalse_QuandoIdadeMaiorOuIgualA18()
    {
        var ctx = CriarContexto();
        var repo = new PessoaRepository(ctx);
        var criada = await repo.CriarAsync(new PessoaRequestDto { Nome = "Ana Dmaior", Idade = 18 });

        Assert.False(criada.MenorIdade);
    }
}
