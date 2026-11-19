namespace AlertaBairro.Domain.Entities;

public class Alerta
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int BairroId { get; set; }

    public Bairro Bairro { get; set; } = null!;
}
