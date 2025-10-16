using MiBlazorMudApp.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiBlazorMudApp.AccessData.ModelConfig;

public class StateConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(e => e.StateId);
        builder.HasIndex(e => new { e.Name, e.CountryId }).IsUnique();
        //protección de borrado en cascada
        builder.HasOne(e => e.Country).WithMany(e => e.States).OnDelete(DeleteBehavior.Restrict);
    }
}