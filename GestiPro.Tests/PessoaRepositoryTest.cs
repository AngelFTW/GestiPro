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
}
