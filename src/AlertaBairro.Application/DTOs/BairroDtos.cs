using System.ComponentModel.DataAnnotations;

namespace AlertaBairro.Application.DTOs;

public class BairroRequest
{
    [Required(ErrorMessage = "O nome do bairro é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do bairro deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "O estado deve conter exatamente 2 letras.")]
    public string Estado { get; set; } = string.Empty;
}

public class BairroResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;
}
