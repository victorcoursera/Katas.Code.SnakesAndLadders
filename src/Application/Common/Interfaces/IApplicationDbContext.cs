using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Board> Boards { get; }

    DbSet<BoardSquare> BoardSquares { get; }

    DbSet<Game> Games { get; }

    DbSet<GamePlayer> GamePlayers { get; }

    DbSet<Player> Players { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
