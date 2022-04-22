using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Infrastructure.Persistence.Configurations;

public class GamePlayerConfiguration : IEntityTypeConfiguration<GamePlayer>
{
    public void Configure(EntityTypeBuilder<GamePlayer> builder)
    {
        builder.HasKey(gp => new { gp.GameId, gp.PlayerId });

        builder.HasOne(gp => gp.Game)
        .WithMany(g => g.GamePlayers)
        .HasForeignKey(gp => gp.GameId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gp => gp.Player)
        .WithMany(p => p.GamePlayers)
        .HasForeignKey(gp => gp.PlayerId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
