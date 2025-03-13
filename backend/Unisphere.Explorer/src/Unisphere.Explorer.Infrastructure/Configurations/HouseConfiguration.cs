using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unisphere.Explorer.Domain;

namespace Infrastructure.Todos;

internal sealed class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder.HasKey(t => t.Id);

        // Notation
        builder
          .OwnsOne(t => t.Notation)
          .Property(pa => pa.GlobalNote)
          .HasColumnName("GlobalNote");

        builder
          .OwnsOne(t => t.Notation)
          .Property(pa => pa.CommunicationNote)
          .HasColumnName("CommunicationNote");

        builder
          .OwnsOne(t => t.Notation)
          .Property(pa => pa.CleanlinessNote)
          .HasColumnName("CleanlinessNote");

        builder
          .OwnsOne(t => t.Notation)
          .Property(pa => pa.LocationNote)
          .HasColumnName("LocationNote");

        // PhysicalAddress
        builder
            .OwnsOne(t => t.PhysicalAddress)
            .Property(pa => pa.City)
            .HasColumnName("City");

        builder
            .OwnsOne(t => t.PhysicalAddress)
            .Property(pa => pa.Street)
            .HasColumnName("Street");

        builder
            .OwnsOne(t => t.PhysicalAddress)
            .Property(pa => pa.ZipCode)
            .HasColumnName("ZipCode");

        builder
            .OwnsOne(t => t.PhysicalAddress)
            .Property(pa => pa.CountryCode)
            .HasColumnName("CountryCode");
    }
}
