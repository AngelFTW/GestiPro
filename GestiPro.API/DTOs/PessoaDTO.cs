using System.ComponentModel.DataAnnotations;

namespace GestiPro.API.DTOs;

public class PessoaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public bool MenorIdade => Idade < 18;
}

public class PessoaRequestDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Idade é obrigatória")]
    [Range(0, 120, ErrorMessage = "Idade inválida. Verifique.")]
    public int Idade { get; set; }
}
