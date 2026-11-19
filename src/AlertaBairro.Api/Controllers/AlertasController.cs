using AlertaBairro.Application.DTOs;
using AlertaBairro.Domain.Entities;
using AlertaBairro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlertaBairro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertasController(AlertaBairroDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertaResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var alertas = await context.Alertas
            .AsNoTracking()
            .Include(alerta => alerta.Bairro)
            .OrderByDescending(alerta => alerta.DataRegistro)
            .Select(alerta => new AlertaResponse
            {
                Id = alerta.Id,
                Titulo = alerta.Titulo,
                Descricao = alerta.Descricao,
                Categoria = alerta.Categoria,
                Status = alerta.Status,
                DataRegistro = alerta.DataRegistro,
                BairroId = alerta.BairroId,
                BairroNome = alerta.Bairro.Nome
            })
            .ToListAsync(cancellationToken);

        return Ok(alertas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlertaResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var alerta = await context.Alertas
            .AsNoTracking()
            .Include(item => item.Bairro)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        return alerta is null
            ? NotFound(new { message = $"Alerta com ID {id} não encontrado." })
            : Ok(ToResponse(alerta));
    }

    [HttpPost]
    public async Task<ActionResult<AlertaResponse>> Create(
        AlertaRequest request,
        CancellationToken cancellationToken)
    {
        var bairroExiste = await context.Bairros
            .AnyAsync(bairro => bairro.Id == request.BairroId, cancellationToken);

        if (!bairroExiste)
        {
            return NotFound(new
            {
                message = $"Bairro com ID {request.BairroId} não encontrado."
            });
        }

        var alerta = new Alerta
        {
            Titulo = request.Titulo.Trim(),
            Descricao = request.Descricao.Trim(),
            Categoria = request.Categoria.Trim(),
            Status = request.Status.Trim(),
            DataRegistro = request.DataRegistro ?? DateTime.UtcNow,
            BairroId = request.BairroId
        };

        context.Alertas.Add(alerta);
        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(alerta)
            .Reference(item => item.Bairro)
            .LoadAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, ToResponse(alerta));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        AlertaRequest request,
        CancellationToken cancellationToken)
    {
        var alerta = await context.Alertas
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (alerta is null)
        {
            return NotFound(new { message = $"Alerta com ID {id} não encontrado." });
        }

        var bairroExiste = await context.Bairros
            .AnyAsync(bairro => bairro.Id == request.BairroId, cancellationToken);

        if (!bairroExiste)
        {
            return NotFound(new
            {
                message = $"Bairro com ID {request.BairroId} não encontrado."
            });
        }

        alerta.Titulo = request.Titulo.Trim();
        alerta.Descricao = request.Descricao.Trim();
        alerta.Categoria = request.Categoria.Trim();
        alerta.Status = request.Status.Trim();
        alerta.DataRegistro = request.DataRegistro ?? alerta.DataRegistro;
        alerta.BairroId = request.BairroId;

        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var alerta = await context.Alertas
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (alerta is null)
        {
            return NotFound(new { message = $"Alerta com ID {id} não encontrado." });
        }

        context.Alertas.Remove(alerta);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static AlertaResponse ToResponse(Alerta alerta) => new()
    {
        Id = alerta.Id,
        Titulo = alerta.Titulo,
        Descricao = alerta.Descricao,
        Categoria = alerta.Categoria,
        Status = alerta.Status,
        DataRegistro = alerta.DataRegistro,
        BairroId = alerta.BairroId,
        BairroNome = alerta.Bairro.Nome
    };
}
