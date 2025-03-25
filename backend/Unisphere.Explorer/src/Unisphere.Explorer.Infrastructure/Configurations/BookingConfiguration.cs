using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unisphere.Core.Common.Extensions;
using Unisphere.Explorer.Domain;

namespace Unisphere.Explorer.Infrastructure.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.CheckOut).HasConversion(d => d.ToDateTime(), d => DateOnly.FromDateTime(d));
    }
}
