using MiBlazorMudApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiBlazorMudApp.AccessData.ModelConfig;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(e => e.CityId);
        builder.HasIndex(e => new { e.Name, e.StateId }).IsUnique();
        //protección de borrado en cascada
        builder.HasOne(e => e.State).WithMany(e => e.Cities).OnDelete(DeleteBehavior.Restrict);
    }
}