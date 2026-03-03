using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GestiPro.API.Models;

namespace GestiPro.API.DTOs;

public class TransacaoResponseDto
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public int IdPessoa { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public string TipoDescricao => Tipo == TipoTransacao.Despesa ? "Despesa" : "Receita";
    public string CategoriaDescricao { get; set; }
    public string PessoaNome { get; set; }
}

public class TransacaoRequestDto
{
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [MaxLength(400, ErrorMessage = "Descrição deve ter no máximo 400 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser positivo")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "Pessoa é obrigatória")]
    public int IdPessoa { get; set; }
}
