using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class PropriedadeConfiguration : IEntityTypeConfiguration<Propriedade>
{
    public void Configure(EntityTypeBuilder<Propriedade> builder)
    {
        builder.ToTable("Propriedade");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProprietarioId).IsRequired();
        builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Descricao).HasMaxLength(500);
        builder.OwnsOne(p => p.Endereco, p =>
        {
            p.Property(e => e.Logradouro).HasColumnName("Logradouro").HasMaxLength(200).IsRequired();
            p.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(15).IsRequired();
            p.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(100);
            p.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(120);
            p.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(120).IsRequired();
            p.Property(e => e.UF).HasColumnName("UF").HasMaxLength(2).IsRequired();
            p.Property(e => e.CEP).HasColumnName("CEP").HasMaxLength(9);
        });
        builder.HasMany(p => p.Talhoes).WithOne(t => t.Propriedade).HasForeignKey(t => t.PropriedadeId).IsRequired();
    }
}
