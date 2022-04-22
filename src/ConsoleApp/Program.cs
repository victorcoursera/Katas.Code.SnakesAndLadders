using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SnakesAndLadders.Application;
using SnakesAndLadders.Application.Games.Commands.CreateGame;
using SnakesAndLadders.Application.Players.Commands.CreatePlayer;
using SnakesAndLadders.Application.Players.Queries.GetPlayerCurrentSituation;
using SnakesAndLadders.Infrastructure;
using SnakesAndLadders.Infrastructure.Persistence;

namespace SnakesAndLadders.ConsoleApp;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
                await ApplicationDbContextSeed.SeedSampleDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }

            await PlayGameAsync(scope);
        }

        //await host.RunAsync();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        var assembly = AppDomain.CurrentDomain.Load("SnakesAndLadders.Application");
        services.AddMediatR(assembly);

        services.AddLogging(config =>
        {
            config.AddDebug();
            config.AddConsole();
        });
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                ConfigureServices(services, hostContext.Configuration);
            });

    private static async Task PlayGameAsync(IServiceScope scope)
    {
        Console.Clear();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        var gameId = await mediator.Send(new CreateGameCommand
        {
            BoardId = 1
        });

        Console.WriteLine("Game 1 created.");

        var player1Id = await mediator.Send(new CreatePlayerCommand
        {
            Nickname = "User1"
        });

        Console.WriteLine("Player User1 created.");

        var player2Id = await mediator.Send(new CreatePlayerCommand
        {
            Nickname = "User2"
        });

        Console.WriteLine("Player User2 created.");

        await mediator.Send(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = player1Id
        });

        await mediator.Send(new AddPlayerToGameCommand
        {
            GameId = gameId,
            PlayerId = player2Id
        });

        await mediator.Send(new StartGameCommand
        {
            GameId = gameId
        });

        Console.WriteLine("Game 1 started.");

        int rollDiceResult = 5;

        await mediator.Send(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = rollDiceResult
        });

        Console.WriteLine("Player User1 rolls a die and the result is " + rollDiceResult + ".");

        GetPlayerCurrentSituationQueryDto playerCurrentSituation = await mediator.Send(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = player1Id
        });

        Console.WriteLine("User1's current position is " + playerCurrentSituation.PlayerCurrentPosition + ".");

        rollDiceResult = 3;

        await mediator.Send(new RollDiceCommand
        {
            GameId = gameId,
            RollDiceResult = rollDiceResult
        });

        Console.WriteLine("Player User2 rolls a die and the result is " + rollDiceResult + ".");

        playerCurrentSituation = await mediator.Send(new GetPlayerCurrentSituationQuery
        {
            GameId = gameId,
            PlayerId = player2Id
        });

        Console.WriteLine("User2's current position is " + playerCurrentSituation.PlayerCurrentPosition + ".");

        Console.ReadKey();
    }
}
