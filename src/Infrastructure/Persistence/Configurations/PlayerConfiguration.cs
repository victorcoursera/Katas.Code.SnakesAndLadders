using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.Property(t => t.Nickname)
            .HasMaxLength(100)
            .IsRequired();
    }
}
