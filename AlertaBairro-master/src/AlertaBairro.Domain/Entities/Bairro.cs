namespace AlertaBairro.Domain.Entities;

public class Bairro
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
}
