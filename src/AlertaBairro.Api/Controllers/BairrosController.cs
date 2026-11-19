using AlertaBairro.Application.DTOs;
using AlertaBairro.Domain.Entities;
using AlertaBairro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlertaBairro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BairrosController(AlertaBairroDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BairroResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var bairros = await context.Bairros
            .AsNoTracking()
            .OrderBy(bairro => bairro.Nome)
            .Select(bairro => new BairroResponse
            {
                Id = bairro.Id,
                Nome = bairro.Nome,
                Cidade = bairro.Cidade,
                Estado = bairro.Estado
            })
            .ToListAsync(cancellationToken);

        return Ok(bairros);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BairroResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var bairro = await context.Bairros
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        return bairro is null
            ? NotFound(new { message = $"Bairro com ID {id} não encontrado." })
            : Ok(ToResponse(bairro));
    }

    [HttpPost]
    public async Task<ActionResult<BairroResponse>> Create(
        BairroRequest request,
        CancellationToken cancellationToken)
    {
        var bairro = new Bairro
        {
            Nome = request.Nome.Trim(),
            Cidade = request.Cidade.Trim(),
            Estado = request.Estado.Trim().ToUpperInvariant()
        };

        context.Bairros.Add(bairro);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = bairro.Id }, ToResponse(bairro));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        BairroRequest request,
        CancellationToken cancellationToken)
    {
        var bairro = await context.Bairros
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (bairro is null)
        {
            return NotFound(new { message = $"Bairro com ID {id} não encontrado." });
        }

        bairro.Nome = request.Nome.Trim();
        bairro.Cidade = request.Cidade.Trim();
        bairro.Estado = request.Estado.Trim().ToUpperInvariant();

        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var bairro = await context.Bairros
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (bairro is null)
        {
            return NotFound(new { message = $"Bairro com ID {id} não encontrado." });
        }

        var possuiAlertas = await context.Alertas
            .AnyAsync(alerta => alerta.BairroId == id, cancellationToken);

        if (possuiAlertas)
        {
            return Conflict(new
            {
                message = "Não é possível excluir um bairro que possui alertas vinculados."
            });
        }

        context.Bairros.Remove(bairro);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static BairroResponse ToResponse(Bairro bairro) => new()
    {
        Id = bairro.Id,
        Nome = bairro.Nome,
        Cidade = bairro.Cidade,
        Estado = bairro.Estado
    };
}
