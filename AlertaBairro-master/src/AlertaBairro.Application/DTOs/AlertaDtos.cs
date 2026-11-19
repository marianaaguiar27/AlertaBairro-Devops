using System.ComponentModel.DataAnnotations;

namespace AlertaBairro.Application.DTOs;

public class AlertaRequest
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(50, ErrorMessage = "A categoria deve ter no máximo 50 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    [StringLength(30, ErrorMessage = "O status deve ter no máximo 30 caracteres.")]
    public string Status { get; set; } = string.Empty;

    public DateTime? DataRegistro { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O BairroId deve ser maior que zero.")]
    public int BairroId { get; set; }
}

public class AlertaResponse
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int BairroId { get; set; }

    public string BairroNome { get; set; } = string.Empty;
}
