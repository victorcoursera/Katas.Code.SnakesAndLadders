using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Application.Common.Interfaces;
using SnakesAndLadders.Domain.Common;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Board> Boards => Set<Board>();

    public DbSet<BoardSquare> BoardSquares => Set<BoardSquare>();

    public DbSet<Game> Games => Set<Game>();

    public DbSet<GamePlayer> GamePlayers => Set<GamePlayer>();

    public DbSet<Player> Players => Set<Player>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
