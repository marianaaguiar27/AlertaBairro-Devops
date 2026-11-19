using AlertaBairro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertaBairro.Infrastructure.Persistence;

public class AlertaBairroDbContext(DbContextOptions<AlertaBairroDbContext> options) : DbContext(options)
{
    public DbSet<Bairro> Bairros => Set<Bairro>();

    public DbSet<Alerta> Alertas => Set<Alerta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bairro>(entity =>
        {
            entity.ToTable("BAIRROS");
            entity.HasKey(bairro => bairro.Id);

            entity.Property(bairro => bairro.Id)
                .ValueGeneratedOnAdd();

            entity.Property(bairro => bairro.Nome)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(bairro => bairro.Cidade)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(bairro => bairro.Estado)
                .HasMaxLength(2)
                .IsRequired();
        });

        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.ToTable("ALERTAS");
            entity.HasKey(alerta => alerta.Id);

            entity.Property(alerta => alerta.Id)
                .ValueGeneratedOnAdd();

            entity.Property(alerta => alerta.Titulo)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(alerta => alerta.Descricao)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(alerta => alerta.Categoria)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(alerta => alerta.Status)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(alerta => alerta.DataRegistro)
                .IsRequired();

            entity.HasOne(alerta => alerta.Bairro)
                .WithMany(bairro => bairro.Alertas)
                .HasForeignKey(alerta => alerta.BairroId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
