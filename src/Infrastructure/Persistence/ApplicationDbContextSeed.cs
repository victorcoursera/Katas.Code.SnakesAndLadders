using Microsoft.EntityFrameworkCore;
using SnakesAndLadders.Domain.Entities;

namespace SnakesAndLadders.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.Boards.Any())
        {
            using var transaction = context.Database.BeginTransaction();
            context.Boards.Add(new Board
            {
                Id = 1
            });
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Boards ON;");
            await context.SaveChangesAsync();

            for (int i = 1; i <= 100; i++)
            {
                BoardSquare boardSquare = new()
                {
                    BoardId = 1,
                    PositionNumber = i
                };
                context.BoardSquares.Add(boardSquare);
            }

            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Boards OFF;");
            await context.SaveChangesAsync();
            transaction.Commit();
        }
    }
}
