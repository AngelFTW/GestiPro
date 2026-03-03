using System.ComponentModel.DataAnnotations;
using GestiPro.API.Models;

namespace GestiPro.API.DTOs;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public CategoriaFinalidade Finalidade { get; set; }
    public string FinalidadeDescricao => Finalidade switch
    {
        CategoriaFinalidade.Despesa => "Despesa",
        CategoriaFinalidade.Receita => "Receita",
        CategoriaFinalidade.Ambas => "Ambas",
        _ => "Desconhecido"
    };
}

public class CategoriaRequestDto
{
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(400, ErrorMessage = "Descrição deve ter no máximo 400 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Finalidade é obrigatória")]
    public CategoriaFinalidade Finalidade { get; set; }
}
