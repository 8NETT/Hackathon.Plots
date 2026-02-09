using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class TalhaoConfiguration : IEntityTypeConfiguration<Talhao>
{
    public void Configure(EntityTypeBuilder<Talhao> builder)
    {
        builder.ToTable("Talhao");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Nome).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Descricao).HasMaxLength(500);
        builder.OwnsOne(t => t.Coordenadas, t =>
        {
            t.Property(c => c.Latitude).HasColumnName("Latitude").IsRequired();
            t.Property(c => c.Longitude).HasColumnName("Longitude").IsRequired();
        });
        builder.Property(t => t.Area).IsRequired();
    }
}
